using DNNE;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OmsiHook;

namespace OmsiHookPlugin
{
    public class OmsiHookPlugin
    {
        private static OmsiHook.OmsiHook hook;

        private static void Log(object msg) => File.AppendAllText("omsiHookPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        public static void PluginStart(IntPtr aOwner)
        {
            try
            {
                File.Delete("omsiHookPluginLog.txt");
            } catch { }
            Log("PluginStart()");
            Log("Loading OmsiHook...");
            hook = new();
            try
            {
                hook.AttachToOMSI().Wait();
            } catch (Exception e) 
            { 
                Log($"Failed to attach to Omsi:\n{e}");
            }
            hook.OnMapLoaded += Hook_OnMapLoaded;
            hook.OnMapChange += Hook_OnMapChange;
            hook.OnOmsiExited += Hook_OnOmsiExited;
            hook.OnOmsiGotD3DContext += Hook_OnOmsiGotD3DContext;
            hook.OnOmsiLostD3DContext += Hook_OnOmsiLostD3DContext;
        }

        private static void Hook_OnOmsiLostD3DContext(object sender, EventArgs e)
        {
            Log($"Lost D3D context!");
        }

        private static void Hook_OnOmsiGotD3DContext(object sender, EventArgs e)
        {
            Log($"Got D3D context!");
        }

        private static void Hook_OnOmsiExited(object sender, EventArgs e)
        {
            Log($"Omsi exited!");
        }

        private static void Hook_OnMapChange(object sender, EventArgs e)
        {
            Log($"Map changed!");
        }

        private static void Hook_OnMapLoaded(object sender, bool e)
        {
            Log($"Map loaded! {e}");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginFinalize))]
        public static void PluginFinalize()
        {
            Log("PluginFinalize()");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessVariable))]
        public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue)
        {
            
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessTrigger))]
        public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }
    }
}
