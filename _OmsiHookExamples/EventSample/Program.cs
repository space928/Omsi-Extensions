﻿using System;
using System.Threading;
using OmsiHook;

namespace EventSample
{
    // Most Basic example of reading various values exposed by OMSIHook
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("#=#=#=#=#=# OMSIHook Events Sample #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();

            Console.WriteLine("Waiting for events...");

            omsi.OnMapChange += Omsi_OnMapChange;
            omsi.OnMapLoaded += Omsi_OnMapLoaded;
            omsi.OnActiveVehicleChanged += Omsi_OnActiveVehicleChanged;
            omsi.OnOmsiExited += Omsi_OnOmsiExited;
            omsi.OnOmsiGotD3DContext += Omsi_OnOmsiGotD3DContext;
            omsi.OnOmsiLostD3DContext += Omsi_OnOmsiLostD3DContext;
            // Await callbacks
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private static void Omsi_OnOmsiLostD3DContext(object? sender, EventArgs e)
        {
            Console.WriteLine("💻 D3D Context Lost");
        }

        private static void Omsi_OnOmsiGotD3DContext(object? sender, EventArgs e)
        {
            Console.WriteLine("💻 D3D Context Secured");
        }

        private static void Omsi_OnOmsiExited(object? sender, EventArgs e)
        {
            Console.WriteLine("🛑 OMSI Has Exited");
        }

        private static void Omsi_OnMapLoaded(object? sender, bool e)
        {
            if (sender != null)
                Console.WriteLine($"🗺️ Map Loaded: {e}");
        }

        private static void Omsi_OnMapChange(object? sender, OmsiMap e)
        {
            if (e != null)
                Console.WriteLine($"🗺️ Map Changed: {e.FriendlyName}");
        }

        private static void Omsi_OnActiveVehicleChanged(object? sender, OmsiRoadVehicleInst e)
        {
            Console.WriteLine($"🚌 Active Vehicle Changed: {e.RoadVehicle.FriendlyName}");
        }
    }
}
