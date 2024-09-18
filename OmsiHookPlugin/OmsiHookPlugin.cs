using DNNE;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OmsiHook;
using static OmsiHook.D3DTexture;
using System.Diagnostics;

namespace OmsiHookPlugin
{
    public class OmsiHookPlugin
    {
        private static OmsiHook.OmsiHook hook;
        private static Stopwatch stopwatch;

        private static OmsiMap map;
        private static OmsiTime time;
        private static OmsiWeather weather;
        private static OmsiRoadVehicleInst playerVehicle;

        private static D3DTexture texture;
        private static int frameCounter;
        private static Task texUpdateTask;

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
            }
            catch { }
            AllocConsole();
            Log("PluginStart()");
            Log("Loading OmsiHook...");
            stopwatch = Stopwatch.StartNew();
            hook = new();
            try
            {
                hook.AttachToOMSI().Wait();
            }
            catch (Exception e)
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

            frameCounter = 0;
        }

        private static void Hook_OnMapLoaded(object sender, bool e)
        {
            Log($"Map loaded! {e}");
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(PluginFinalize))]
        public static void PluginFinalize()
        {
            Log("PluginFinalize()");

            // Clean up any resources we may have acquired
            texUpdateTask?.Wait();
            texture?.Dispose();
            hook?.Dispose();
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) }, EntryPoint = nameof(AccessVariable))]
        public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue)
        {
            try
            {
                map ??= hook.Globals.Map;
                time ??= hook.Globals.Time;
                weather ??= hook.Globals.Weather;
                playerVehicle ??= hook.Globals.PlayerVehicle;
                var pos = playerVehicle?.Position ?? default;
                var rot = playerVehicle?.Rotation ?? default;

                Console.SetCursorPosition(0, 8);
                Console.WriteLine($"Map: {map?.FriendlyName}".PadRight(Console.WindowWidth - 17) + $"Date: {time.Day:00}/{time.Month:00}/{time.Year:0000}");
                Console.WriteLine($"Weather: {weather?.ActWeather.name}".PadRight(Console.WindowWidth - 15) + $"Time: {time.Hour:00}:{time.Minute:00}:{time.Second:00}");
                Console.WriteLine($"Bus: {playerVehicle?.RoadVehicle?.FileName}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Position: {pos}".PadRight(Console.WindowWidth - 10) + $"Tile: {playerVehicle?.Kachel ?? 0,3}");
                Console.WriteLine($"Rotation: {rot}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine("\nPress T to replace all script textures. Press S to replace all string vars.\n");
            }
            catch (Exception ex)
            {
                Log($"Error while printing status information:\n{ex}");
            }

            frameCounter++;

            if (!Console.KeyAvailable)
                return;

            var lastKey = Console.ReadKey(true);
            stopwatch.Restart();

            try
            {
                switch (lastKey.Key)
                {
                    case ConsoleKey.T:
                        UpdateScriptTexture();
                        break;
                    case ConsoleKey.S:
                        UpdateStringVars();
                        break;
                }

                Log($"Success! t={frameCounter} elapsed={stopwatch.Elapsed}");
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private static void UpdateScriptTexture()
        {
            // Wait until the current texture update task has finished
            if (!texUpdateTask?.IsCompleted ?? false)
                return;

            uint width = 1024, height = 1024;
            OmsiRemoteMethods.D3DFORMAT fmt = OmsiRemoteMethods.D3DFORMAT.D3DFMT_A8R8G8B8;
            if (texture == null)
            {
                texture = hook.CreateTextureObject();
                texture.CreateD3DTexture(width, height, fmt, 1, true).Wait();

                var scriptTextures = hook.Globals.PlayerVehicle.ComplObjInst.ScriptTextures;
                for (int i = 0; i < scriptTextures.Count; i++)
                {
                    var oldST = scriptTextures[i];
                    Log($"  ScriptTex {i}: col = #{oldST.color:X8}; ptr = 0x{oldST.tex:X8}; TexPn = [{(oldST.TexPn == null ? "null" : string.Join(", ", oldST.TexPn))}]");
                    scriptTextures[0] = new()
                    {
                        color = oldST.color,
                        tex = (IntPtr)texture.TextureAddress,
                        TexPn = oldST.TexPn
                    };
                }
            }

            RGBA[] pixels = new RGBA[width * height];
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Make a nice pattern on the texture
                    (int x1, int y1) = (x - (int)width / 2, y - (int)height / 2);
                    uint p = (uint)(x1 * x1 + y1 * y1 + frameCounter);
                    uint rgba = p + (uint)((Math.Abs(x1) + Math.Abs(y1) - frameCounter - 256) << 17);
                    pixels[index++].data = rgba;
                }
            }

            texUpdateTask = texture.UpdateTexture(pixels.AsMemory());
        }

        private static void UpdateStringVars()
        {
            Console.WriteLine($"First 10 string vars on the player vehicle:");
            var svarNames = playerVehicle?.ComplMapObj?.SVarStrings?.Take(10);
            if (svarNames == null)
                return;
            foreach (string svar in svarNames)
            {
                Console.WriteLine($"\t{svar} = {playerVehicle?.GetStringVariable(svar)}");
            }
            string nval = $"Test-{frameCounter}";
            Console.WriteLine($"Setting new value = '{nval}'...");
            foreach (string svar in svarNames)
            {
                playerVehicle?.SetStringVariable(svar, nval);
                Console.WriteLine($"\t{svar} = {playerVehicle?.GetStringVariable(svar)}");
            }
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
