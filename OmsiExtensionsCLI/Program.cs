using System;
using System.Threading;
using OmsiHook;
using System.Threading.Tasks;

namespace OmsiExtensionsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OmsiExtensions Testing #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            Task.WaitAll(omsi.AttachToOMSI());
            Console.Clear();

            while (true)
            {
                var pos = omsi.Globals.PlayerVehicle.Position;
                var posa = omsi.Globals.PlayerVehicle.AbsPosition;
                var vel = omsi.Globals.PlayerVehicle.Velocity;
                var map = omsi.Globals.Map;
                var weather = omsi.Globals.Weather;
                var tickets = omsi.Globals.TicketPack;
                var humans = omsi.Globals.Humans;

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(($"Read data: x:{pos.x:F3}   y:{pos.y:F3}   z:{pos.z:F3}      " +
                    $"tile:{omsi.Globals.PlayerVehicle.Kachel}").PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Read data: vx:{vel.x:F3}   vy:{vel.y:F3}   vz:{vel.z:F3}".PadRight(Console.WindowWidth-1));
                Console.WriteLine($"Read data: ax:{posa._30:F3}   ay:{posa._31:F3}   az:{posa._32:F3}".PadRight(Console.WindowWidth-1));

                Console.WriteLine($"Read data: map:{map.Name}   path:{map.Filename}   friendly:{map.FriendlyName}".PadRight(Console.WindowWidth-1));
                Console.WriteLine($"{omsi.Globals.PlayerVehicle.PAI_LastBrake} {omsi.Globals.PlayerVehicle.Bremspedal}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"{omsi.Globals.Time.Day}/{omsi.Globals.Time.Month}/{omsi.Globals.Time.Year} - {omsi.Globals.Time.Hour}:{omsi.Globals.Time.Minute}:{omsi.Globals.Time.Second:F2}");
                Console.WriteLine("".PadRight(Console.WindowWidth-1));
                //omsi.Globals.PlayerVehicle.Velocity = new D3DVector { x=0, y=0, z=5 };
                //omsi.Globals.PlayerVehicle.Bremspedal = 0;

                Thread.Sleep(50);
            }
        }
    }
}
