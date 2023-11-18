using System;
using System.Threading;
using OmsiHook;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX;
using System.Runtime.InteropServices;

namespace OmsiExtensionsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OmsiExtensions Testing #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            Console.Clear();
            DXTests dXTests = new();
            dXTests.Init(omsi);
            bool toggle = false;
            while (true)
            {
                var pos = omsi.Globals.PlayerVehicle?.Position ?? default;
                var posa = omsi.Globals.PlayerVehicle?.AbsPosition ?? default;
                var vel = omsi.Globals.PlayerVehicle?.Velocity ?? default;
                var map = omsi.Globals.Map;
                var cam = omsi.Globals.Camera;
                var camPos = cam?.Pos ?? default;
                var weather = omsi.Globals.Weather;
                var tickets = omsi.Globals.TicketPack;
                var humans = omsi.Globals.Humans;

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(($"Read data: x:{pos.x:F3}   y:{pos.y:F3}   z:{pos.z:F3}      " +
                    $"tile:{omsi.Globals.PlayerVehicle?.Kachel??0}").PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Read data: vx:{vel.x:F3}   vy:{vel.y:F3}   vz:{vel.z:F3}".PadRight(Console.WindowWidth-1));
                Console.WriteLine($"Read data: ax:{posa._30:F3}   ay:{posa._31:F3}   az:{posa._32:F3}".PadRight(Console.WindowWidth-1));

                Console.WriteLine($"Read data: map:{map?.Name}   path:{map?.Filename}   friendly:{map?.FriendlyName}".PadRight(Console.WindowWidth-1));
                Console.WriteLine($"{omsi.Globals.Time.Day}/{omsi.Globals.Time.Month}/{omsi.Globals.Time.Year} - {omsi.Globals.Time.Hour}:{omsi.Globals.Time.Minute}:{omsi.Globals.Time.Second:F2}");
                Console.WriteLine($"Camera data: x:{camPos.x:F3}   y:{camPos.y:F3}   z:{camPos.z:F3}      ".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"{omsi.Globals.Drivers}".PadRight(Console.WindowWidth - 1));

                var tex = dXTests.CreateTexture();
                dXTests.UpdateTexture(tex);

                Console.WriteLine("".PadRight(Console.WindowWidth-1));
                try
                {
                    Console.WriteLine($"INEO_PS_Matricule: {omsi.Globals.PlayerVehicle.GetStringVariable("INEO_Login")}".PadRight(Console.WindowWidth - 1));


                    omsi.Globals.PlayerVehicle.SetStringVariable("INEO_Login", toggle ? "thomas" : "01234");
                    toggle = !toggle;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }

                Thread.Sleep(200);
            }
        }
    }

    public class DXTests
    {
        private readonly ManualResetEventSlim d3dGotContext = new(false);
        private OmsiHook.OmsiHook omsi;
        private bool ready = false;
        private SharpDX.Direct3D11.Device device;
        private const int texWidth = 256;
        private const int texHeight = 256;
        private RGBA[] texBuffer = new RGBA[texWidth * texHeight];

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 4)]
        private struct RGBA
        {
            [FieldOffset(0)]public byte r; 
            [FieldOffset(1)]public byte g; 
            [FieldOffset(2)]public byte b;
            [FieldOffset(3)]public byte a;

            [FieldOffset(0)]public uint data;
        }

        public void Init(OmsiHook.OmsiHook omsi)
        {
            this.omsi = omsi;
            omsi.OnOmsiGotD3DContext += Omsi_OnOmsiGotD3DContext;
            omsi.OnMapLoaded += Omsi_OnMapLoaded;
        }

        private void Hook()
        {
            ready = false;
            d3dGotContext.Wait();
            while (true)
            {
                if (OmsiRemoteMethods.OmsiHookD3D())
                    break;
            }
            ready = true;
        }

        public uint CreateTexture()
        {
            if(!ready)
                Hook();

            (uint hresult, uint texturePtr) = OmsiRemoteMethods.OmsiCreateTextureAsync(texWidth, texHeight, OmsiRemoteMethods.D3DFORMAT.D3DFMT_A8B8G8R8).Result;
            if (hresult != 0)
                throw new Exception("Couldn't create D3D texture! Result: " + new SharpDX.Result(hresult));

            var scriptTexes = omsi.Globals.PlayerVehicle.ComplObjInst.ScriptTextures;
            for (int i = 0; i < scriptTexes.Count; i++)
            {
                var old = scriptTexes[i];
                scriptTexes[i] = new()
                {
                    TexPn = old.TexPn,
                    color = old.color,
                    tex = (IntPtr)texturePtr
                };
            }

            //device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.None);
            //return device.OpenSharedResource<SharpDX.Direct3D11.Texture2D>((IntPtr)textureHandle);
            return texturePtr;
        }

        public void UpdateTexture(uint texturePtr)
        {
            uint texMemPtr = OmsiRemoteMethods.OmsiGetMem(texWidth * texHeight * 4).Result;
            uint[] managedTextureBuffer = new uint[texWidth * texHeight * 4];
            for (int y = 0; y < texHeight; y++)
                for(int x = 0; x < texWidth; x++)
                {
                    managedTextureBuffer[x + y * texWidth] = new RGBA()
                    {
                        r = (byte)((x * 4) % 256),
                        g = (byte)((y * 4) % 256),
                        b = (byte)(((x + y) * 4) % 256),
                        a = 255
                    }.data;
                }

#if DEBUG
            omsi.OmsiMemory.WriteMemory(texMemPtr, managedTextureBuffer);
#else
            Console.WriteLine("WARNING: Currently UpdateTexture() only works in debug builds.");
#endif

            int hr = unchecked((int)OmsiRemoteMethods.OmsiUpdateTextureAsync(texturePtr, texMemPtr, texWidth, texHeight).Result);
            if(hr != 0) 
                throw new SharpDXException(hr);
        }

        private void Omsi_OnOmsiGotD3DContext(object sender, EventArgs e)
        {
            // d3dGotContext.Set();
        }

        private void Omsi_OnMapLoaded(object sender, bool e)
        {
            d3dGotContext.Set();
        }
    }
}
