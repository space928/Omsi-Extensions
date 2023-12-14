#include "pch.h"
#include "FunctionHooks.h"

FunctionHooks::FunctionHooks()
{
	DWORD oldProtect;
	VirtualProtect(hookTrampolineFunc, sizeof(hookTrampolineFunc), PAGE_EXECUTE_READWRITE, &oldProtect);
	ZeroMemory(hookTrampolineFunc, sizeof(hookTrampolineFunc));
}

FunctionHooks::~FunctionHooks()
{

}

void FunctionHooks::InstallHook(UINT32 funcToHook)
{
	DWORD oldProtect;
	VirtualProtect((LPVOID)funcToHook, 256, PAGE_EXECUTE_READWRITE, &oldProtect);
	byte jmpInstruction[5] = { 0xE9, 0x0, 0x0, 0x0, 0x0 };

	const UINT32 relAddr = (UINT32)hookTrampolineFunc - (funcToHook + sizeof(jmpInstruction));
	memcpy(jmpInstruction + 1, &relAddr, 4);
	//https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-flushinstructioncache
	memcpy((LPVOID)funcToHook, jmpInstruction, sizeof(jmpInstruction));
	VirtualProtect((LPVOID)funcToHook, 256, oldProtect, &oldProtect);
}

#define AsLEBytes(addr) ((addr>>3)&0xff), ((addr>>2)&0xff), ((addr>>1)&0xff), ((addr>>0)&0xff)

void FunctionHooks::OnTrigger(void(*callback)(LPCSTR trigger, int value))
{
	// Bouncy bounce
	UINT32 callbackRelAddr = (UINT32)callback - ((UINT32)hookTrampolineFunc + 11);
	UINT32 returnRelAddr = (UINT32)0x007bae96 - ((UINT32)hookTrampolineFunc + 25);
	// Setup the hook handler so that it calls the callback and then jumps back to the original function
	byte hookTrampolineTemp[] = {
		// Save the register args for later
		0x52, //push eax
		0x55, //push edx
		0x54, //push ecx

		// Convert a borland fastcall to stdcall
		0x52, //push eax
		0x55, //push edx
		0x54, //push ecx
		0xe8, AsLEBytes(callbackRelAddr), //call callback

		// Restore register args
		0x59, //pop ecx
		0x5a, //pop edx
		0x57, //pop eax

		// Now execute the prolog of the function we overwrote and jump back
		0x55, //push ebp
		0x8b, 0xec, //mov ebp, esp
		0x83, 0xc4, 0xf0,//add esp, -0x10
		0xe9, AsLEBytes(returnRelAddr) //jmp 0x007bae96
	};
	memcpy(hookTrampolineFunc, hookTrampolineTemp, sizeof(hookTrampolineTemp));

	InstallHook(0x007bae90);
}
