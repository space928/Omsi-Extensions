// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

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
    case DLL_PROCESS_DETACH:
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
/// <param name="*varargs*">Arguments to pass into the function</param>
/// <returns></returns>
__declspec(naked) int _cdecl BorlandFastCall(int funcPtr, int nRegArgs, ...)
{
    __asm
    {
        //Don't preamble to avoid having to copy lots of bits of the stack around
        /*
        * At this point the stack should look like:
        * | *old stack frames*
        * | argN
        * | ...
        * | arg0            <= ESP+0xc
        * | nRegArgs        <= ESP+0x8
        * | funcPtr         <= ESP+0x4
        * | returnAddress   <= ESP
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
        
        mov ecx, [esp+0x8]  // Load nRegArgs

        cmp ecx, 0x1        // If there is at least 1 argument, move it to eax
        jl FIXSTACK_PARTIAL
        mov eax, [esp+0xc]

        cmp ecx, 0x2        // If there are at least 2 arguments, move it to edx
        jl FIXSTACK_PARTIAL
        mov edx, [esp+0x10]

        cmp ecx, 0x3        // If there are at least 3 arguments, move it to ecx
        jl FIXSTACK_PARTIAL
        mov ecx, [esp+0x14]
        jmp FIXSTACK_FULL   // All three args had to be moved, as such ecx no longer
                            // contains nRegArgs


        FIXSTACK_PARTIAL:   // When this is called the number of args that were moved 
                            // is in ecx
        imul ecx, 0x4       // Make ecx point just after the first argument on the stack:
        lea ecx, [esp+ecx]  //      ecx = ESP - (nRegArgs*0x4) - 0x8
        add ecx, 0x8        // Subtract funcPtr and nRegArgs from the pointer

        mov [ecx-0x18], ebx // Save EBX to a part of the stack we are certain is free

        mov ebx, [esp]
        mov [ecx], ebx      // Move the return address to just before the 
                            // first argument on the stack
        sub ecx, 0x4
        mov ebx, [esp+0x4]
        mov [ecx], ebx      // Move the function address to just after that
        lea esp, [ecx+0x4]  // Set the stack pointer to point to the return address

        mov ebx, [esp-0x18] // Restore EBX
        jmp END


        FIXSTACK_FULL:      // When this is called the number of args that were moved 
                            // is exactly 0x3
        mov [esp-0x4], ebx  // Save EBX to a part of the stack we are certain is free
        add esp, 0x10       // Move the stack pointer to just after the new location
                            // of the return address on the stack
        mov ebx, [esp-0xc]  // Move the function pointer
        mov [esp], ebx 
        add esp, 0x4        // Move the stack pointer to just after the first argument
        mov ebx, [esp-0x14] // Move the return address 
        mov [esp], ebx

        mov ebx, [esp-0x18] // Restore EBX


        END:
        /*
        * At this point the stack should look like:
        * | *old stack frames*
        * | (argN)
        * | (...)
        * | (arg3)          <= ESP+0x4
        * | returnAddress   <= ESP
        * | funcPtr         <= ESP-0x4
        * | saved EBX       <= ESP-0x8
        * | *empty stack*
        */

        jmp [esp-0x4]       // Called the Borland Fastcall function
        ret                 // Technically superfluous, since we set the return address to
                            // the return of the calling method
    }
}

// Warning function signature types have little meaning, they are chosen specifically to 
// ensure the correct argument size is used!
extern "C" __declspec(dllexport) int TProgManMakeVehicle(int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
    int TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
    int kennzeichen_index, bool initcall, int startday, UINT8 trainbuilddir, bool reverse,
    int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
    bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename)
{
    return BorlandFastCall(0x0070a250, 3, 
        progMan, vehList, _RoadVehicleTypes, onlyvehlist, CS,
        TTtime, situationload, dialog, setdriver, thread,
        kennzeichen_index, initcall, startday, trainbuilddir, reverse,
        grouphof, typ, tour, line, farbschema, Scheduled,
        AIRoadVehicle, kennzeichen_random, farbschema_random, filename);
}
