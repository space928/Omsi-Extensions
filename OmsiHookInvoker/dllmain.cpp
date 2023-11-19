// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "DXHook.h"

DXHook* m_dxHook;

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        if (m_dxHook)
            delete m_dxHook;
        break;
    }
    return TRUE;
}

/// <summary>
/// Calls a function at a given address using the BorlandFastcall calling convention.
/// </summary>
/// <param name="funcPtr">The pointer to the function to call</param>
/// <param name="nRegArgs">The number of arguments to pass in through registers.
/// Real, method-pointer, variant, Int64, and structured types do not count as register 
/// arguments; all other types do.</param>
/// <param name="nArgs">The total number of arguments the function takes. This really 
/// refers to the number of double words (4 byte) values passed in. 64 bit types such as
/// double/Int64 count for two arguments. Arguments smaller than a dword still count as
/// an entire argument in this case.</param>
/// <param name="*varargs*">Arguments to pass into the function</param>
/// <returns></returns>
__declspec(naked) int _cdecl BorlandFastCall(int funcPtr, int nRegArgs, int nArgs, ...)
{
    __asm
    {
        push ebp                // Setup a new stack frame and leave us some space for locals
        mov ebp, esp        
        sub esp, 0x10           // Reserve space for 4 local variables

        /*
        * At this point the stack should look like:
        * | *old stack frames*
        * | argN
        * | ...
        * | arg0            <= EBP+0x14
        * | nArgs           <= EBP+0x10
        * | nRegArgs        <= EBP+0xc
        * | funcPtr         <= EBP+0x8
        * | returnAddress   <= EBP+0x4
        * | prevEBP         <= EBP
        * | *local_4*       <= EBP-0x4
        * | *local_8*       <= EBP-0x8
        * | *local_c*       <= EBP-0xc
        * | *local_10*      <= ESP
        * | *empty stack*
        *
        * The Borland Fastcall convention puts the first three arguments into
        * EAX, EDX, ECX respectively. This bit of assembly moves the first three args
        * (if they exist) into these registers and then calls the function.
        * Using pop and push with our unconventional stack setup (we're trying to avoid
        * needing to copy all the arguments accross while reordering things on the
        * stack) will lead to pain; as such we don't touch ebp and leave esp until we've 
        * finished shuffling.
        */

        // Save EBX, ESI, and EDI to use later
        push ebx
        push esi
        push edi

        // Copy arguments accross to be passed to the external function
        mov ebx, [ebp+0x10]     // Load nArgs
        sub ebx, [ebp+0xc]      // ebx = nArgs-nRegArgs
        mov esi, [ebp+0x10]     // esi = &arg0+nArgs = &argN
        lea esi, [ebp+0x10+esi*0x4]

        ARG_COPY_LOOP:          // Copy accross #EBX arguments
        cmp ebx, 0x0
        jz ARG_COPY_LOOP_END
        // Get the next argument and copy into our stack frame
        push [esi]

        sub esi, 0x4            // Increment iteration variables
        dec ebx
        jmp ARG_COPY_LOOP

        ARG_COPY_LOOP_END:
        // Load register arguments
        mov ebx, [ebp+0xc]      // Load nRegArgs

        cmp ebx, 0x1            // If there is at least 1 argument, move it to eax
        jl FINISH_COPYING_REG_ARGS
        mov eax, [ebp+0x14]

        cmp ebx, 0x2            // If there are at least 2 arguments, move it to edx
        jl FINISH_COPYING_REG_ARGS
        mov edx, [ebp+0x18]

        cmp ebx, 0x3            // If there are at least 3 arguments, move it to ecx
        jl FINISH_COPYING_REG_ARGS
        mov ecx, [ebp+0x1c]

        FINISH_COPYING_REG_ARGS:

        call [ebp+0x8]          // Called the Borland Fastcall function
        // Restore the stack
        // Note that Borland Fastcall tidies up some of the stack for us.
        // Restore EBX, ESI, and EDI
        pop edi
        pop esi
        pop ebx

        add esp, 0x10           // Restore the space taken by the 4 variables we allocated (which I know I didn't use in the end...)
        mov esp, ebp            // Restore the stack pointer to the top of the previous frame
        pop ebp                 // Restore the previous stack frame
        
        ret                     // Ret pops the return address which should now be at the top of the stack
    }
}

// Warning function signature types have little meaning, they are chosen specifically to 
// ensure the correct argument size is used!
// NOTE: For float params, the compiler likes to extend them to doubles sometimes, just make 
// them ints in the function signature to avoid weirdness
extern "C" __declspec(dllexport) int TProgManMakeVehicle(int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
    int TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
    int kennzeichen_index, bool initcall, int startday, UINT8 trainbuilddir, bool reverse,
    int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
    bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename)
{
    return BorlandFastCall(0x0070a250, 3, 25, 
        progMan, vehList, _RoadVehicleTypes, onlyvehlist, CS,
        TTtime, situationload, dialog, setdriver, thread,
        kennzeichen_index, initcall, startday, trainbuilddir, reverse,
        grouphof, typ, tour, line, farbschema, Scheduled,
        AIRoadVehicle, kennzeichen_random, farbschema_random, filename);
}

extern "C" __declspec(dllexport) int TTempRVListCreate(int classAddr, int capacity)
{
    return BorlandFastCall(0x0074a0e0, 2, 2, 
        classAddr, capacity);
}

extern "C" __declspec(dllexport) int TProgManPlaceRandomBus(int progMan, int aityp, 
    int group, int TTtime, bool thread, bool instantCopy, int _typ,
    bool scheduled, int startDay, int tour, int line)
{
    return BorlandFastCall(0x00708f8c, 3, 11, 
        progMan, aityp, group, TTtime, thread, instantCopy, _typ,
        scheduled, startDay, tour, line);
}

extern "C" __declspec(dllexport) int GetMem(int length)
{
    return BorlandFastCall(0x00404614, 1, 1,
        length);
}

extern "C" __declspec(dllexport) int FreeMem(int addr)
{
    return BorlandFastCall(0x00404630, 1, 1,
        addr);
}

extern "C" __declspec(dllexport) BOOL HookD3D()
{
    if (!m_dxHook)
        m_dxHook = new DXHook();

    return m_dxHook->HookD3D();
}

extern "C" __declspec(dllexport) HRESULT CreateTexture(UINT Width, UINT Height, D3DFORMAT Format, IDirect3DTexture9** ppTexture)
{
    if (!m_dxHook)
        return E_FAIL;
    return m_dxHook->CreateTexture(Width, Height, Format, ppTexture);
}

extern "C" __declspec(dllexport) HRESULT UpdateSubresource(IDirect3DTexture9* Texture, UINT8* TextureData, UINT Width, UINT Height, BOOL UseRect, LONG32 Left, LONG32 Top, LONG32 Right, LONG32 Bottom)
{
    if (!m_dxHook)
        return E_FAIL;
    return m_dxHook->UpdateSubresource(Texture, TextureData, Width, Height, UseRect, Left, Top, Right, Bottom);
}

extern "C" __declspec(dllexport) HRESULT ReleaseTexture(IDirect3DTexture9* Texture)
{
    return DXHook::ReleaseTexture(Texture);
}

extern "C" __declspec(dllexport) HRESULT GetTextureDesc(IDirect3DTexture9 * Texture, UINT * pWidth, UINT * pHeight, UINT * pFormat)
{
    return DXHook::GetTextureDesc(Texture, pWidth, pHeight, pFormat);
}

extern "C" __declspec(dllexport) HRESULT IsTexture(IUnknown* Texture)
{
    return DXHook::IsTexture(Texture);
}
