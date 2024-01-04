#pragma once
class FunctionHooks
{
public:
	FunctionHooks();
	~FunctionHooks();
	void OnTrigger(void(*callback)(int complMapObjInst, LPCSTR trigger, int value));

private:
	void InstallHook(UINT32 funcToHook, BYTE* hookTrampolineFunc);
};

