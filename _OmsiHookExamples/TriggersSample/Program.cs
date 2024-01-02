using System;
using System.Threading;
using OmsiHook;

namespace TriggersSample
{
    // Basic sample of Triggers with OMSIHook
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OMSIHook Trigger Sample #=#=#=#=#=#");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();

            var playerVehicle = omsi.Globals.PlayerVehicle;
            bool triggerState = false;

            while (true)
            {
                playerVehicle ??= omsi.Globals.PlayerVehicle;
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"Trigger State: {triggerState}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Playing Sound...".PadRight(Console.WindowWidth - 1));
                playerVehicle.SetTrigger("bus_doorfront0", triggerState);
                playerVehicle.SoundTrigger("ev_IBIS_Ansagen", @"..\..\MAN_NL_NG\Sound\Matrix_Ziel.wav");
                triggerState = !triggerState;
                Thread.Sleep(1000);
            }
        }
    }
}
