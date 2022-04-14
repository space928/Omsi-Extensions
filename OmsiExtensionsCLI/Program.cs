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

            OmsiHook.OmsiHook omsi = new OmsiHook.OmsiHook();
            omsi.AttachToOMSI();

            while (true)
            {
                var pos = omsi.PlayerVehicle.Position;

                Console.WriteLine($"Read data: x:{pos.x:F3}\ty:{pos.y:F3}\tz:{pos.z:F3}\t\t" +
                    $"tile:{0}\trow45:{0:F3}\trow47:{0:F3}");

                Thread.Sleep(500);
            }
        }
    }
}
