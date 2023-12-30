using System;
using System.Threading;
using OmsiHook;

namespace Trigger_Sample
{
    // Basic sample of Triggers with OMSIHook
    class Program
    {
        OmsiHook.OmsiHook omsi;
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OMSIHook Trigger Sample #=#=#=#=#=#");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            var playerVehicle = omsi.Globals.PlayerVehicle;
            bool trigger_state = false;

            Thread.Sleep(500);
            while (true)
            {
                playerVehicle ??= omsi.Globals.PlayerVehicle;
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"Trigger State: {trigger_state}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Playing Sound...".PadRight(Console.WindowWidth - 1));
                playerVehicle.SetTrigger("bus_doorfront0", trigger_state);
                playerVehicle.SoundTrigger("ev_IBIS_Ansagen", @"..\..\MAN_NL_NG\Sound\Matrix_Ziel.wav");
                trigger_state = !trigger_state;
                Thread.Sleep(1000);
            }
        }
    }
}
