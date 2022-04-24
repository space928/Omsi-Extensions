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

        private static void Log(string msg) => File.AppendAllText("omsiHookPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        public static void PluginStart(IntPtr aOwner)
        {
            File.Delete("omsiHookPluginLog.txt");
            Log("PluginStart()");
            Log("Loading OmsiHook...");
            hook = new();
            hook.AttachToOMSI();
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
                    hook.RemoteMethods.MakeVehicle();
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

        [DllImport("OmsiHookInvoker.dll")]
        private static extern int TProgManMakeVehicle(int progMan, int vehList, int _RoadVehicleTypes, bool onlyvehlist, bool CS,
            float TTtime, bool situationload, bool dialog, bool setdriver, bool thread,
            int kennzeichen_index, bool initcall, int startday, byte trainbuilddir, bool reverse,
            int grouphof, int typ, int tour, int line, int farbschema, bool Scheduled,
            bool AIRoadVehicle, bool kennzeichen_random, bool farbschema_random, int filename);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int TTempRVListCreate(int classAddr, int capacity);
        [DllImport("OmsiHookInvoker.dll")]
        private static extern int TProgManPlaceRandomBus(int progMan, int aityp,
    int group, float TTtime, bool thread, bool instantCopy, int _typ,
    bool scheduled, int startDay, int tour, int line);
    }
}
