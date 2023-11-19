#pragma once

#include "pch.h"

#define CHECK_FAILURE_RETURN_FILE_LINE(x, file, line) { HRESULT __hr = (x); if(FAILED(__hr)) return __hr; }
// Checks a function returning an HRESULT for failure. If it doesn't return S_OK, the hresult is returned
#define CHECK_FAILURE_RETURN(x) CHECK_FAILURE_RETURN_FILE_LINE(x, __FILE__, __LINE__)

#define _FACOMSIHOOKRPC  0x554
#define MAKE_OHHRESULT( code )  MAKE_HRESULT( 1, _FACOMSIHOOKRPC, code )
#define MAKE_OHSTATUS( code )  MAKE_HRESULT( 0, _FACOMSIHOOKRPC, code )

#define OHERR_NOD3DDEVICE                            MAKE_OHHRESULT(10)
#define OHERR_D3DDEVICEQUERYFAILED                   MAKE_OHHRESULT(11)
#define OHERR_UPDATESUBRES_TEXTURENULL               MAKE_OHHRESULT(20)
#define OHERR_UPDATESUBRES_DSTTEXTURETOOSMALL        MAKE_OHHRESULT(21)
#define OHERR_UPDATESUBRES_INVALIDRECT               MAKE_OHHRESULT(22)
