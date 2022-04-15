# Omsi-Extensions
Omsi hooking and modding sdk.

The dead simple way to hook into Omsi and fiddle with it's memory. In it's current state we only have mappings for a limited number of Omsi objects, but it's easy to extend. Allows both reading and writing data from Omsi in real time, but currently doesn't allow for anything that requires memory allocation (eg: adding elements to arrays).

The library makes use of C# properties and classes to wrap native Omsi objects making them simple to interact with in a managed environment.

## Usage Example
Here's a quick example of how to use it.
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
            var pos = omsi.PlayerVehicle.Position;
            
            // Print the position to the console
            Console.WriteLine($"Player vehicle pos: x:{pos.x:F3}\ty:{pos.y:F3}\tz:{pos.z:F3}");

            Thread.Sleep(500);
        }
    }
}
```
