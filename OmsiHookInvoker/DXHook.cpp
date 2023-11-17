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
		return FALSE;
	}
	if (m_device == nullptr)
		return FALSE;
	m_device->AddRef();
	return TRUE;
}

HRESULT DXHook::CreateTexture(UINT Width, UINT Height, D3DFORMAT Format, IDirect3DTexture9** ppTexture, HANDLE* pSharedHandle)
{
	if (!m_device)
		return E_FAIL;
	if ((int)m_device == -2)
		return -2;

	return m_device->CreateTexture(Width, Height, 1, D3DUSAGE_RENDERTARGET, Format, D3DPOOL_DEFAULT, ppTexture, pSharedHandle);
}
