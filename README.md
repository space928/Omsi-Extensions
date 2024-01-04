# Omsi-Extensions
Omsi hooking and modding sdk.

[![.NET](https://github.com/space928/Omsi-Extensions/actions/workflows/dotnet.yml/badge.svg)](https://github.com/space928/Omsi-Extensions/actions/workflows/dotnet.yml)
[![DocFX](https://github.com/space928/Omsi-Extensions/actions/workflows/docs.yml/badge.svg)](https://space928.github.io/Omsi-Extensions/index.html)
[![Discord](https://img.shields.io/discord/1192462752527163503?logo=Discord&logoColor=fff&label=Discord)](https://discord.gg/FG9P6PW23w)
[![Nuget](https://img.shields.io/nuget/v/omsihook?logo=Nuget&logoColor=fff)](https://www.nuget.org/packages/OmsiHook/)
[![OMSI Version](https://img.shields.io/badge/OMSI%20Version-2.3.004-orange?logo=steam&logoColor=fff)](https://store.steampowered.com/app/252530/OMSI_2_Steam_Edition/)

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
        // Create an OmsiHook and attach to any running instance of OMSI
        OmsiHook.OmsiHook omsi = new();
        omsi.AttachToOMSI().Wait();

        while (true)
        {
            // Get the PlayerVehicle and get its position
            var pos = omsi.Globals.PlayerVehicle.Position;
            
            // Print the position to the console
            Console.WriteLine($"Player vehicle pos: {pos}");

            Thread.Sleep(500);
        }
    }
}
```
See the `OmsiExtensionsCLI` [Program.cs](OmsiExtensionsCLI/Program.cs) for more examples using OmsiHook.

## Project Structure
This repository contains the source for the OmsiHook and OmsiHookInvoker libraries as well as template 
projects using OmsiHook which run under various frameworks.

Here's a summary of the project structure:
```
\Omsi-Extensions\
┃
┠─► \_OmsiHookExamples\    -> A collection of sample projects demonstrating various OmsiHook
┃                             features.
┠─► \OmsiHook\             -> Base library containing all the Omsi hooking code and 
┃                             exposing Omsi's internal data.
┠─► \OmsiHookInvoker\      -> C++ plugin for invoking native Omsi methods from OmsiHook, 
┃                             only used by OmsiHook.
┠─► \OmsiHookRPCPlugin\    -> An Omsi plugin which exposes native methods from OmsiHookInvoker 
┃                             to other processes using OmsiHook.
┠─► \OmsiExtensionsCLI\    -> Example command line application that uses OmsiHook; runs
┃                             outside of Omsi.
┠─► \OmsiExtensionsUI\     -> Example Avalonia UI (similar to WPF) application that uses
┃                             OmsiHook; runs outside of Omsi.
┖─► \OmsiHookPlugin\       -> Example plugin that uses OmsiHook and compiles to a native
                              Omsi plugin by using DNNE.
```

## Building
The project requires .NET 6 SDK to build and only works on Windows x86_32. Because of the nature of 
the project dependencies, sometimes Visual Studio gets the build order wrong (especially for projects 
depending on OmsiHookInvoker). When updating to a new version of OmsiHook or if you make changes to 
OmsiHookInvoker you may need to clean and rebuild the solution; if you're really struggling delete the
contents of the following directories:
```
Omsi-Extensions\Debug
Omsi-Extensions\Release
Omsi-Extensions\OmsiExtensionsCLI\bin\
Omsi-Extensions\OmsiHookPlugin\bin\
```
The project can be configured to automatically copy binaries to your Omsi directory. To do so set the 
`OmsiDir` environment variable to your Omsi directory (eg: `set "OmsiDir=C:\Program Files\OMSI 2\"`).
You can also set it by editing the Post-build event action in the `OmsiHookRPCPlugin.csproj` and 
`OmsiHookPlugin.csproj` project files. Note that the binaries are only copied when `OmsiHookRPCPlugin` 
or `OmsiHookPlugin` is rebuilt.
