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
	m_device = (IDirect3DDevice9*)*((void**)OMSI_D3DCONTEXT_PTR);
	if (m_device == nullptr)
		return FALSE;
	m_device->AddRef();
	return TRUE;
}

HRESULT DXHook::CreateTexture(UINT Width, UINT Height, D3DFORMAT Format, IDirect3DTexture9** ppTexture, HANDLE* pSharedHandle)
{
	if (!m_device)
		return E_FAIL;

	return m_device->CreateTexture(Width, Height, 1, D3DUSAGE_DYNAMIC, Format, D3DPOOL_DEFAULT, ppTexture, pSharedHandle);
}
