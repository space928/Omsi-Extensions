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
		return E_FAIL;
	if ((int)m_device == -2)
		return -2;

	// Shared handle is only supported with D3D9Ex sadly:
	// https://www.gamedev.net/forums/topic/638495-shared-resources-eg-textures-between-devicesthreads/5030524/
	
	return m_device->CreateTexture(Width, Height, 1, D3DUSAGE_DYNAMIC, Format, D3DPOOL_DEFAULT, ppTexture, NULL);
}

HRESULT DXHook::UpdateSubresource(IDirect3DTexture9* Texture, UINT8* TextureData, UINT Width, UINT Height, BOOL UseRect, LONG32 Left, LONG32 Top, LONG32 Right, LONG32 Bottom)
{
	// Check the arguments, users can never be trusted and this is annoying to debug...
	if (Texture == nullptr || TextureData == nullptr)
		return E_ABORT;

	HRESULT hr;
	D3DSURFACE_DESC surfaceDesc;
	hr = Texture->GetLevelDesc(0, &surfaceDesc);
	if(FAILED(hr))
		return hr;

	if (surfaceDesc.Width < Width || surfaceDesc.Height < Height)
		return E_INVALIDARG;

	if (UseRect)
		if (Right - Left != Width || Bottom - Top != Height)
			return E_INVALIDARG;

	const RECT rect {
		Left,
		Top,
		Right,
		Bottom
	};
	D3DLOCKED_RECT lockedRect;
	hr = Texture->LockRect(0, &lockedRect, UseRect ? &rect : NULL, D3DLOCK_DISCARD);
	if (FAILED(hr))
		return hr;

	for (UINT y = 0; y < Height; y++)
	{
		void* dst = ((UINT8*)lockedRect.pBits) + (y * lockedRect.Pitch);
		memcpy(dst, (TextureData + Width * y), Width);
	}

	hr = Texture->UnlockRect(0);

	return hr;
}
