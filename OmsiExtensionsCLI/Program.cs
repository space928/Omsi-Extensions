using System;
using System.Threading;
using OmsiHook;

namespace OmsiExtensionsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OmsiExtensions Testing #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI();

            while (true)
            {
                var pos = omsi.PlayerVehicle.Position;
                var map = omsi.Map;
                var weather = omsi.Weather;
                var tickets = omsi.TicketPack;

                Console.WriteLine($"Read data: x:{pos.x:F3}\ty:{pos.y:F3}\tz:{pos.z:F3}\t\t" +
                    $"tile:{0}\trow45:{0:F3}\trow47:{0:F3}");
                
                Console.WriteLine($"Read data: map:{map.Name}\tpath:{map.Filename}\tfriendly:{map.FriendlyName}");
                Console.WriteLine($"Read data: act w name:{weather.ActWeather.name}");

                Thread.Sleep(500);
            }
        }
    }
}
