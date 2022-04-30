# Omsi-Extensions
Omsi hooking and modding sdk.

[![.NET](https://github.com/space928/Omsi-Extensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/space928/Omsi-Extensions/actions/workflows/dotnet.yml)
[![DocFX](https://github.com/space928/Omsi-Extensions/actions/workflows/docs.yml/badge.svg)](https://github.com/space928/Omsi-Extensions/actions/workflows/docs.yml)
![Nuget](https://img.shields.io/nuget/v/omsihook)
![OMSI Version](https://img.shields.io/badge/OMSI%20Version-2.3.004-orange)

The dead simple way to hook into Omsi and fiddle with it's memory. In it's current state we only have 
mappings for a limited number of Omsi objects, but it's easy to extend. Allows both reading and writing 
data from Omsi in real time, but currently doesn't allow for anything that requires memory allocation 
(eg: adding elements to arrays).

The library makes use of C# properties and classes to wrap native Omsi objects making them simple to interact 
with in a managed environment.

See the [documentation](https://space928.github.io/Omsi-Extensions/index.html) for details on the API.

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

## Project Structure
This repository contains the source for the OmsiHook and OmsiHookInvoker libraries as well as template 
projects using OmsiHook which run under various frameworks.

Here's a summary of the project structure:
```
\Omsi-Extensions\
||
||=> \OmsiHook\             -> Base library containing all the Omsi hooking code and 
||                             exposing Omsi's internal data.
||=> \OmsiHookInvoker\      -> C++ plugin for invoking native Omsi methods from OmsiHook, 
||                             only used by OmsiHook.
||=> \OmsiExtensionsCLI\    -> Example command line application that uses OmsiHook; runs
||                             outside of Omsi.
||=> \OmsiExtensionsUI\     -> Example Avalonia UI (similar to WPF) application that uses
||                             OmsiHook; runs outside of Omsi.
||=> \OmsiHookPlugin\       -> Example plugin that uses OmsiHook and compiles to a native
                               Omsi plugin by using DNNE.
```
