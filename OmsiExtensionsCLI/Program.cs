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
                var pos = omsi.PlayerVehicle.Position;
                var posa = omsi.PlayerVehicle.AbsPosition;
                var vel = omsi.PlayerVehicle.Velocity;
                var map = omsi.Map;
                var weather = omsi.Weather;
                var tickets = omsi.TicketPack;

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(($"Read data: x:{pos.x:F3}   y:{pos.y:F3}   z:{pos.z:F3}      " +
                    $"tile:{omsi.PlayerVehicle.Kachel}").PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Read data: vx:{vel.x:F3}   vy:{vel.y:F3}   vz:{vel.z:F3}".PadRight(Console.WindowWidth-1));
                Console.WriteLine($"Read data: ax:{posa._30:F3}   ay:{posa._31:F3}   az:{posa._32:F3}".PadRight(Console.WindowWidth-1));

                Console.WriteLine($"Read data: map:{map.Name}   path:{map.Filename}   friendly:{map.FriendlyName}".PadRight(Console.WindowWidth-1));
                Console.WriteLine($"{omsi.PlayerVehicle.PAI_LastBrake} {omsi.PlayerVehicle.Bremspedal}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"{omsi.Date_Day}/{omsi.Date_Month}/{omsi.Date_Year} - {omsi.Time_Hour}:{omsi.Time_Minute}:{omsi.Time_Second:F2}");
                Console.WriteLine("".PadRight(Console.WindowWidth-1));
                //omsi.PlayerVehicle.Velocity = new D3DVector { x=0, y=0, z=5 };
                //omsi.PlayerVehicle.Bremspedal = 0;

                Thread.Sleep(50);
            }
        }
    }
}
