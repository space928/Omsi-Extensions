#pragma once

class DXHook
{
private:
	const UINT32 OMSI_D3DCONTEXT_PTR = 0x008627d0;

	IDirect3DDevice9* m_device;
    
public:
	DXHook();

	~DXHook();

	BOOL HookD3D();

	HRESULT CreateTexture(UINT Width, UINT Height, D3DFORMAT Format, IDirect3DTexture9** ppTexture, HANDLE* pSharedHandle);
};
