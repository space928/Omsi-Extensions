using System;
using System.Threading;
using OmsiHook;

namespace Basic_CLI
{
    // Most Basic example of reading various values exposed by OMSIHook
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#=#=#=#=#=# OMSIHook Basic CLI Sample #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();
            var playerVehicle = omsi.Globals.PlayerVehicle;
            var time = omsi.Globals.Time;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                playerVehicle ??= omsi.Globals.PlayerVehicle;
                var pos = playerVehicle?.Position ?? default;
                var rot = playerVehicle?.Rotation ?? default;
                var map = omsi.Globals.Map;
                var weather = omsi.Globals.Weather;

                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"Map: {map?.FriendlyName}".PadRight(Console.WindowWidth - 17) + $"Date: {time.Day:00}/{time.Month:00}/{time.Year:0000}");
                Console.WriteLine($"Weather: {weatherEmoji(weather)}".PadRight(Console.WindowWidth - 15) + $"Time: {time.Hour:00}:{time.Minute:00}:{time.Second:00}");
                Console.WriteLine($"Bus: {playerVehicle?.RoadVehicle?.FileName}".PadRight(Console.WindowWidth - 1));
                Console.WriteLine(($"Position: {pos.x:F2},{pos.y:F2},{pos.z:F2}".PadRight(Console.WindowWidth - 8) +
                    $"Tile: {playerVehicle?.Kachel ?? 0}"));
                Console.WriteLine(($"Rotation: {rot.w:F2},{rot.x:F2},{rot.y:F2},{rot.z:F2}".PadRight(Console.WindowWidth - 1)));

                Thread.Sleep(20);
            }
        }
        static string weatherEmoji(OmsiWeather weather)
        {
            if (weather?.ActWeather.fogDensity < 900)
                return "🌫️";
            if (weather?.ActWeather.percipitation > 0)
                return "🌧️";
            return "☀️";
        }
    }
}
