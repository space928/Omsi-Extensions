# Basic Trigger & Sound Trigger Example

This article provides a basic understanding to a basic C# .NET example leveraging the OMSIHook library. The example focuses on retrieving crucial information about the map, weather, date, and the current vehicle.

_This article is in direct relation to the Sample Project available [here](https://github.com/space928/Omsi-Extensions/tree/main/_OmsiHookExamples/BasicCLI)._

## Initialization

Initialize an instance of the `OmsiHook` class and establish a connection to the OMSI game:

```cs
OmsiHook.OmsiHook omsi = new();
omsi.AttachToOMSI().Wait();
```

## Caching of Globals

Efficiently cache top-level global variables outside the loop for optimized performance:

```cs
// Cache global variables for faster access
var playerVehicle = omsi.Globals.PlayerVehicle;
var time = omsi.Globals.Time;
var map = omsi.Globals.Map;
var weather = omsi.Globals.Weather;
```

By caching these variables outside the loop, you significantly enhance access speed during subsequent iterations.
