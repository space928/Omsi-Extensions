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
        // OK so here's the issue, cdecl expects the caller to pop used args off the stack after calling, but BorlandFastcall does it in the callee.
        // This means the stack is cleaned twice which is bad. We have to use cdecl on this method to support varargs and currently doing things
        // after jumping to the external function is difficult (since we jump instead of call, because this method doesn't set up it's own stack frame)
    }
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
__declspec(naked) int _cdecl BorlandFastCall2(int funcPtr, int nRegArgs, int nArgs, ...)
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
    return BorlandFastCall2(0x0070a250, 3, 25, 
        progMan, vehList, _RoadVehicleTypes, onlyvehlist, CS,
        TTtime, situationload, dialog, setdriver, thread,
        kennzeichen_index, initcall, startday, trainbuilddir, reverse,
        grouphof, typ, tour, line, farbschema, Scheduled,
        AIRoadVehicle, kennzeichen_random, farbschema_random, filename);
}

extern "C" __declspec(dllexport) int TTempRVListCreate(int classAddr, int capacity)
{
    return BorlandFastCall2(0x0074a0e0, 2, 2, 
        classAddr, capacity);
}

extern "C" __declspec(dllexport) int TProgManPlaceRandomBus(int progMan, int aityp, 
    int group, int TTtime, bool thread, bool instantCopy, int _typ,
    bool scheduled, int startDay, int tour, int line)
{
    return BorlandFastCall2(0x00708f8c, 3, 11, 
        progMan, aityp, group, TTtime, thread, instantCopy, _typ,
        scheduled, startDay, tour, line);
}
