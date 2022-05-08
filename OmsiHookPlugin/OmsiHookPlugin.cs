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

        private static void Log(object msg) => File.AppendAllText("omsiHookPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        public static void PluginStart(IntPtr aOwner)
        {
            File.Delete("omsiHookPluginLog.txt");
            Log("PluginStart()");
            Log("Loading OmsiHook...");
            hook = new();
            _ = hook.AttachToOMSI();
            Log("Didn't crash!");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginFinalize))]
        public static void PluginFinalize()
        {
            Log("PluginFinalize()");
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
                    hook.RemoteMethods.PlaceRandomBus();
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

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessTrigger))]
        public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }
    }
}
