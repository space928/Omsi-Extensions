# OmsiHook Documentation
## Getting Started
1. Download the NuGet package
[https://www.nuget.org/packages/OmsiHook/](https://www.nuget.org/packages/OmsiHook/)
2. Start using OmsiHook
```cs
using OmsiHook;

class Program
{
    static void Main(string[] args)
    {
        // Create an OmsiHook and attach to any running instance of Omsi
        OmsiHook.OmsiHook omsi = new();
        omsi.AttachToOMSI();

        while (true)
        {
            // Get the PlayerVehicle and get its position
            var pos = omsi.Globals.PlayerVehicle.Position;
            
            // Print the position to the console
            Console.WriteLine($"Player vehicle pos: x:{pos.x:F3}\ty:{pos.y:F3}\tz:{pos.z:F3}");

            Thread.Sleep(500);
        }
    }
}
```