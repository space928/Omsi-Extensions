using System;
using System.Threading;
using OmsiHook;

namespace BasicCLI
{
    // Most basic example of reading various values exposed by OMSIHook
    class Program
    {
        static OmsiRoadVehicleInst? playerVehicle;
        
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OMSIHook Basic CLI Sample #=#=#=#=#=#");

            // Attach OmsiHook to the running instance of Omsi; this method is async and will complete when a running
            // instance of Omsi is detected and has been connected to. We can wait synchronously for the method to
            // complete by either using the await operator or calling it's Wait() method.
            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            omsi.OnActiveVehicleChanged += (_, inst) => playerVehicle = inst;

            // Set the console output encoding to support emoji
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var time = omsi.Globals.Time;
            var map = omsi.Globals.Map;
            var weather = omsi.Globals.Weather;
            playerVehicle = omsi.Globals.PlayerVehicle;
            while (true)
            {
                // These variables will return null until they have a valid value (they will become null if Omsi is in
                // the title screen or is changing map). By using the null assignment operator, we can continously
                // check if the object is valid, if so we get it only if it doesn't already exist; which saves
                // constructing a new wrapper object every iteration of the while loop.
                map ??= omsi.Globals.Map;
                time ??= omsi.Globals.Time;
                weather ??= omsi.Globals.Weather;
                var pos = playerVehicle?.Position ?? default;
                var rot = playerVehicle?.Rotation ?? default;

                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"Map: {map?.FriendlyName}".PadRight(Console.WindowWidth - 17) + $"Date: {time.Day:00}/{time.Month:00}/{time.Year:0000}");
                Console.WriteLine($"Weather: {WeatherEmoji(weather)}".PadRight(Console.WindowWidth - 15) + $"Time: {time.Hour:00}:{time.Minute:00}:{time.Second:00}");
                Console.WriteLine($"Bus: {playerVehicle?.RoadVehicle?.FileName}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine($"Position: {pos}".PadRight(Console.WindowWidth - 10) + $"Tile: {playerVehicle?.Kachel ?? 0, 3}");
                Console.WriteLine(($"Rotation: {rot}".PadRight(Console.WindowWidth - 1)));

                Thread.Sleep(20);
            }
        }

        // Pick an emoji to show for the weather
        static string WeatherEmoji(OmsiWeather? weather)
        {
            if (weather == null)
                return "N/A";
            if (weather?.ActWeather.fogDensity < 900)
                return "🌫️";
            if (weather?.ActWeather.percipitation > 0)
                return "🌧️";
            return "☀️";
        }
    }
}
