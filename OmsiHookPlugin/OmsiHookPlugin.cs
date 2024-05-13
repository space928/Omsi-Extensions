using DNNE;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OmsiHook;
using static OmsiHook.D3DTexture;

namespace OmsiHookPlugin
{
    public class OmsiHookPlugin
    {
        private static OmsiHook.OmsiHook hook;

        private static void Log(object msg)
        {
            string msgStr = $"[{DateTime.Now:dd/MM/yy HH:mm:ss:ff}] {msg}\n";
            File.AppendAllText("omsiHookPluginLog.txt", msgStr);
            Console.Write(msgStr);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginStart))]
        public static void PluginStart(IntPtr aOwner)
        {
            try
            {
                File.Delete("omsiHookPluginLog.txt");
            } catch { }
            AllocConsole();
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

        private static void Hook_OnMapChange(object sender, OmsiMap e)
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

        private static D3DTexture texture;
        private static int counter;

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessVariable))]
        public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue)
        {
            // TEMP
            Console.WriteLine("Update");

            if (!Console.KeyAvailable)
                return;

            var lastKey = Console.ReadKey(true);

            try
            {
                uint width = 1024, height = 1024;
                OmsiRemoteMethods.D3DFORMAT fmt = OmsiRemoteMethods.D3DFORMAT.D3DFMT_A8R8G8B8;
                if (null == texture)
                {
                    texture = hook.CreateTextureObject();
                    texture.CreateD3DTexture(width, height, fmt, true).Wait();
                    //_texture.InitialiseForWriting().Wait();

                    var scriptTextures = hook.Globals.PlayerVehicle.ComplObjInst.ScriptTextures;
                    foreach (var texture in scriptTextures)
                    {
                        Log($"  st {texture.tex}");
                    }

                    var oldTexture = scriptTextures[0];
                    scriptTextures[0] = new() { color = oldTexture.color, tex = (IntPtr)texture.TextureAddress, TexPn = oldTexture.TexPn };
                }

                //var image = Image.Load<Rgba32>(@"D:\Program Files\OMSI 2\Vehicles\GPM_C2\Texture\envmap_unscharf.bmp");

                byte[] pixels = new byte[width * height * 4];
                int index = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //Rgba32 pixel = image[x, y];
                        pixels[index++] = (byte)(x%256);
                        pixels[index++] = (byte)(y%256);
                        pixels[index++] = (byte)(((x+y+counter) %16)*16);
                        pixels[index++] = 255;
                    }
                }
                counter++;

                texture.UpdateTexture(pixels.AsMemory()).Wait();

                //image.Dispose();

                Log("Success!");
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            // TEMP
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessTrigger))]
        public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}
