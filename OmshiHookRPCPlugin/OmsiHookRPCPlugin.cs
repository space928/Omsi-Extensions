using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO.Pipes;
using DNNE;

namespace OmsiHookRPCPlugin
{
    public class OmsiHookRPCPlugin
    {
        public const string PIPE_NAME = "OmsiHookRPCPipe";

        private static OmsiHook.OmsiHook hook;
        private static NamedPipeServerStream pipe;

        private static void Log(object msg) => File.AppendAllText("omsiHookRPCPluginLog.txt", $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n");

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        // ReSharper disable once UnusedMember.Global
        public static void PluginStart(IntPtr aOwner)
        {
            File.Delete("omsiHookRPCPluginLog.txt");
            Log("PluginStart()");
            Log("Loading OmsiHook...");
            hook = new();
            _ = hook.AttachToOMSI();
            Log("Opening name pipe...");

            pipe = new(PIPE_NAME, PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginFinalize))]
        // ReSharper disable once UnusedMember.Global
        public static void PluginFinalize()
        {
            Log("Exiting OmsiHookRPCPlugin...");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessVariable))]
        public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessTrigger))]
        public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }
    }
}
