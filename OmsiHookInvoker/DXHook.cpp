#include "pch.h"
#include "DXHook.h"

DXHook::DXHook()
{
	m_device = nullptr;
}

DXHook::~DXHook()
{
	m_device->Release();
}

BOOL DXHook::HookD3D()
{
    if (m_device != nullptr)
        return TRUE;
        //m_device->Release();

	IUnknown* device = (IUnknown*)*((void**)OMSI_D3DDEVICE_PTR);
	if (device->QueryInterface<IDirect3DDevice9>(&m_device) != S_OK)
	{
		m_device = (IDirect3DDevice9*) -2;
		OutputDebugStringA("[OmsiHookRPC] Query interface for d3ddevice failed!");
		return FALSE;
	}

	/*IDirect3D9Ex* d3d;
	Direct3DCreate9Ex(D3D_SDK_VERSION, &d3d);
	d3d->CreateDeviceEx(0, D3DDEVTYPE::D3DDEVTYPE_HAL, )*/

	if (m_device == nullptr)
		return FALSE;
	m_device->AddRef();
	return TRUE;
}

HRESULT DXHook::CreateTexture(UINT Width, UINT Height, D3DFORMAT Format, IDirect3DTexture9** ppTexture)
{
	if (!m_device)
		return OHERR_NOD3DDEVICE; 
	if ((int)m_device == -2)
		return OHERR_D3DDEVICEQUERYFAILED;

	// Shared handle is only supported with D3D9Ex sadly:
	// https://www.gamedev.net/forums/topic/638495-shared-resources-eg-textures-between-devicesthreads/5030524/
	
	return m_device->CreateTexture(Width, Height, 1, D3DUSAGE_DYNAMIC, Format, D3DPOOL_DEFAULT, ppTexture, NULL);
}

HRESULT DXHook::UpdateSubresource(IDirect3DTexture9* Texture, UINT8* TextureData, UINT Width, UINT Height, BOOL UseRect, LONG32 Left, LONG32 Top, LONG32 Right, LONG32 Bottom)
{
	// Check the arguments, users can never be trusted and this is annoying to debug...
	if (Texture == nullptr || TextureData == nullptr)
		return OHERR_TEXTURENULL;

    D3DSURFACE_DESC desc;
    CHECK_FAILURE_RETURN(Texture->GetLevelDesc(0, &desc));
	/*IDirect3D9* d3d;
	CHECK_FAILURE_RETURN(m_device->GetDirect3D(&d3d));
	D3DDISPLAYMODE displayMode;
	CHECK_FAILURE_RETURN(d3d->GetAdapterDisplayMode(D3DADAPTER_DEFAULT, &displayMode));
	CHECK_FAILURE_RETURN(d3d->CheckDeviceFormat(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, displayMode.Format, desc.Usage, D3DRTYPE_TEXTURE, desc.Format));*/

	if (desc.Width < Width || desc.Height < Height)
		return OHERR_UPDATESUBRES_DSTTEXTURETOOSMALL;

	if (UseRect)
		if (Right - Left != Width || Bottom - Top != Height)
			return OHERR_UPDATESUBRES_INVALIDRECT;

	const RECT rect {
		Left,
		Top,
		Right,
		Bottom
	};
	D3DLOCKED_RECT lockedRect;
	CHECK_FAILURE_RETURN(Texture->LockRect(0, &lockedRect, UseRect ? &rect : NULL, 0));

    int bpp = BitsPerPixel(desc.Format) / 8;
	for (UINT y = 0; y < Height; y++)
	{
		void* dst = ((UINT8*)lockedRect.pBits) + (y * lockedRect.Pitch);
		memcpy(dst, (TextureData + y * Width * bpp), Width * bpp);
	}

	CHECK_FAILURE_RETURN(Texture->UnlockRect(0));

	return S_OK;
}

HRESULT DXHook::ReleaseTexture(IDirect3DTexture9* Texture)
{
    if (Texture == nullptr)
        return OHERR_TEXTURENULL;
    return Texture->Release();
}

HRESULT DXHook::GetTextureDesc(IDirect3DTexture9* Texture, UINT* pWidth, UINT* pHeight, UINT* pFormat)
{
    if (Texture == nullptr)
        return OHERR_TEXTURENULL;
    D3DSURFACE_DESC desc;
    CHECK_FAILURE_RETURN(Texture->GetLevelDesc(0, &desc));
    *pWidth = desc.Width;
    *pHeight = desc.Height;
    *pFormat = desc.Format;
    return S_OK;
}

HRESULT DXHook::IsTexture(IUnknown* Texture)
{
    if (Texture == nullptr)
        return OHERR_TEXTURENULL;
    IDirect3DTexture9* itex;
    return Texture->QueryInterface(&itex);
}

//-------------------------------------------------------------------------------------
// Returns bits-per-pixel for a given D3D9 format, or 0 on failure
//-------------------------------------------------------------------------------------
constexpr size_t DXHook::BitsPerPixel(D3DFORMAT fmt) noexcept
{
    switch (static_cast<int>(fmt))
    {
        // 4 bpp
    case D3DFMT_DXT1:
    case D3DFMT_DXT2:
    case D3DFMT_DXT3:
    case D3DFMT_DXT4:
    case D3DFMT_DXT5:
        return 4;
        // 8 bpp
    case D3DFMT_R3G3B2:
    case D3DFMT_A8:
    case D3DFMT_P8:
    case D3DFMT_L8:
        return 8;
        // 16 bpp
    case D3DFMT_R5G6B5:
    case D3DFMT_X1R5G5B5:
    case D3DFMT_A1R5G5B5:
    case D3DFMT_A4R4G4B4:
    case D3DFMT_A8R3G3B2:
    case D3DFMT_X4R4G4B4:
    case D3DFMT_A8P8:
    case D3DFMT_A8L8:
    case D3DFMT_A4L4:
    case D3DFMT_V8U8:
    case D3DFMT_L6V5U5:
    case D3DFMT_R8G8_B8G8:
    case D3DFMT_YUY2:
    case D3DFMT_G8R8_G8B8:
    case D3DFMT_D16_LOCKABLE:
    case D3DFMT_D15S1:
    case D3DFMT_D16:
    case D3DFMT_L16:
    case D3DFMT_INDEX16:
    case D3DFMT_R16F:
    case D3DFMT_CxV8U8:
        return 16;
        // 24 bpp
    case D3DFMT_R8G8B8:
        return 24;
        // 32 bpp
    case D3DFMT_A8R8G8B8:
    case D3DFMT_X8R8G8B8:
    case D3DFMT_A2B10G10R10:
    case D3DFMT_A8B8G8R8:
    case D3DFMT_X8B8G8R8:
    case D3DFMT_G16R16:
    case D3DFMT_A2R10G10B10:
    case D3DFMT_X8L8V8U8:
    case D3DFMT_Q8W8V8U8:
    case D3DFMT_V16U16:
    case D3DFMT_A2W10V10U10:
    case D3DFMT_UYVY:
    case D3DFMT_D32:
    case D3DFMT_D24S8:
    case D3DFMT_D24X8:
    case D3DFMT_D24X4S4:
    case D3DFMT_D32F_LOCKABLE:
    case D3DFMT_D24FS8:
    case D3DFMT_INDEX32:
    case D3DFMT_MULTI2_ARGB8: // Maybe?
    case D3DFMT_G16R16F:
    case D3DFMT_R32F:
        return 32;
        // 64 bpp
    case D3DFMT_A16B16G16R16:
    case D3DFMT_Q16W16V16U16:
    case D3DFMT_A16B16G16R16F:
    case D3DFMT_G32R32F:
        return 64;
        // 128 bpp
    case D3DFMT_A32B32G32R32F:
        return 128;
        // 0 bpp
    case D3DFMT_UNKNOWN:
    case D3DFMT_VERTEXDATA:
        return 0;
            /* D3D9Ex only -- */
#if !defined(D3D_DISABLE_9EX)
    case D3DFMT_D32_LOCKABLE:
        return 32;
    case D3DFMT_S8_LOCKABLE:
        return 8;
    case D3DFMT_A1:
        return 1;
    case D3DFMT_A2B10G10R10_XR_BIAS:
    case D3DFMT_BINARYBUFFER:
        return 0;
#endif // !D3D_DISABLE_9EX
    default:
        return 0;
    }
}
