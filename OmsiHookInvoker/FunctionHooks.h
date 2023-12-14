#pragma once
class FunctionHooks
{
public:
	FunctionHooks();
	~FunctionHooks();
	void OnTrigger(void (*callback) (LPCSTR trigger, int value));

private:
	void InstallHook(UINT32 funcToHook);

	byte hookTrampolineFunc[256];
};

