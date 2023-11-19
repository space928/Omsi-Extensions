#pragma once

class DXHook
{
private:
	const UINT32 OMSI_D3DDEVICE_PTR = 0x008627d0;

	IDirect3DDevice9* m_device;
    
public:
	DXHook();

	~DXHook();

	BOOL HookD3D();

	HRESULT CreateTexture(UINT Width, UINT Height, D3DFORMAT Format, IDirect3DTexture9** ppTexture);

	HRESULT UpdateSubresource(IDirect3DTexture9* Texture, UINT8* TextureData, UINT Width, UINT Height, BOOL UseRect, LONG32 Left, LONG32 Top, LONG32 Right, LONG32 Bottom);

	static HRESULT ReleaseTexture(IDirect3DTexture9* Texture);

	static HRESULT GetTextureDesc(IDirect3DTexture9* Texture, UINT* pWidth, UINT* pHeight, UINT* pFormat);

	static HRESULT IsTexture(IUnknown* Texture);

	static constexpr size_t BitsPerPixel(D3DFORMAT fmt) noexcept;
};
