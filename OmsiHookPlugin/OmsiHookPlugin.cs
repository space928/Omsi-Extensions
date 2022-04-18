using DNNE;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OmsiHook;
using System.Security;
using System.Diagnostics;

namespace OmsiHookPlugin
{
    public class OmsiHookPlugin
    {
        private static OmsiHook.OmsiHook hook;
        private static bool spawned = false;

        private static readonly byte[] CALL_CONVENTION_FIXUP = { 0x5B, 0x5F, 0x58, 0x5A, 0x59, 0x53, 0xFF, 0xD7, 0xBF, 0xFF, 0xFF, 0xFF, 0xFF, 0xC3 };
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int CallConventionFixup(int address, int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
              float TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
              int kennzeichen_index, bool initcall, int startday, byte trainbuilddir, bool reverse,
              int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
              bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename);
        private static CallConventionFixup callConventionFixup;
        private static void CreateCallConventionFixup()
        {
            var ptr = Marshal.AllocHGlobal(CALL_CONVENTION_FIXUP.Length);
            Marshal.Copy(CALL_CONVENTION_FIXUP, 0, ptr, CALL_CONVENTION_FIXUP.Length);
            VirtualProtectEx(Process.GetCurrentProcess().Handle, ptr,
                (UIntPtr)CALL_CONVENTION_FIXUP.Length, 0x40, out uint _);
            callConventionFixup = Marshal.GetDelegateForFunctionPointer<CallConventionFixup>(ptr);
        }

        private static void Log(string msg) => File.AppendAllText("omsiHookPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");

        delegate void testInvoke(int thisObj, int param2);
        public static void TestInvoke()
        {
            Marshal.GetDelegateForFunctionPointer<testInvoke>(new IntPtr(0x0083b824))
                .DynamicInvoke(hook.ReadMemory(0x008591e8), 0);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate int MakeVehicle(int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
              float TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
              int kennzeichen_index, bool initcall, int startday, byte trainbuilddir, bool reverse,
              int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
              bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename);
        public static void SpawnBusTest()
        {
           // Marshal.GetDelegateForFunctionPointer<MakeVehicle>(new IntPtr(0x0070a250))
            callConventionFixup(0x0070a250, hook.ReadMemory(0x00862f28), hook.ReadMemory(0x0086150C), 
                hook.ReadMemory(0x008615A8), false, false,
              0, false, false, true, false,
              -1, true, 0, (byte)3, false,
              0, 0, 0, 0, 0, false,
              true, true, true, 0);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        public static void PluginStart(IntPtr aOwner)
        {
            Log("PluginStart()");
            Log("Loading OmsiHook...");
            hook = new();
            hook.AttachToOMSI();
            Log("Creating call convention fixer...");
            try
            {
                CreateCallConventionFixup();
            } catch (Exception ex)
            {
                Log(ex.ToString());
            }

            Log("Didn't crash!");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginFinalize))]
        public static void PluginFinalize()
        {
            Log("PluginFinalize()");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessTrigger))]
        public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript)
        {

        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessVariable))]
        public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue)
        {
            var val = Marshal.PtrToStructure<float>(value);
            if (val > 0.9 && !spawned)
            {
                spawned = true;
                Log("Spawning bus!");
                try
                {
                    SpawnBusTest();
                } catch (Exception e)
                {
                    Log("Uh oh:");
                    Log(e.Message);
                    Log(e.ToString());
                }
                
                Log("The bus might have spawned!");
            } else
            {
                if (val < 0.1)
                    spawned = false;
            }
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue)
        {

        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue)
        {
            
        }

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int AssemblyAddFunction(int x, int y);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
    }
}
