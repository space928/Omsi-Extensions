# Basic Events Example

This article provides a basic understanding to a basic C# .NET example leveraging the OMSIHook library. The example focuses using the event handlers provided by OMSIHook.

_This article is in direct relation to the Sample Project available [here](https://github.com/space928/Omsi-Extensions/tree/main/_OmsiHookExamples/EventSample)._

## Basic Usage
Similar to most C# .NET event handlers, you simply need to append the handler to the field as demonstraited below.

```cs
OmsiHook.OmsiHook omsi = new();
omsi.OnMapChange += Omsi_OnMapChange;
omsi.OnMapLoaded += Omsi_OnMapLoaded;
omsi.OnActiveVehicleChanged += Omsi_OnActiveVehicleChanged;
omsi.OnOmsiExited += Omsi_OnOmsiExited;
omsi.OnOmsiGotD3DContext += Omsi_OnOmsiGotD3DContext;
omsi.OnOmsiLostD3DContext += Omsi_OnOmsiLostD3DContext;
```

## Event Descriptions
The events all provide the current `OMSIHook` object as well as several of them provide references to the object that has changed.

### _MapChange_ Event
Triggered any time the current map has changed.
```cs
Omsi_OnMapChange(object? sender, OmsiMap e)
```
The `sender` object as with all the other events refers to the current `OMSIHook` object, the `e` object is a reference to the new `OmsiMap` object or `null`.

### _MapLoaded_ Event
Triggered any time a map is loaded or unloaded.
```cs
Omsi_OnMapLoaded(object? sender, bool e)
```
The `sender` object as with all the other events refers to the current `OMSIHook` object, the `e` is a bool that contains weather or not the event if for a map loading or unloading.

### _ActiveVehicleChanged_ Event
Triggered any time a player changes vehicle.
```cs
Omsi_OnActiveVehicleChanged(object? sender, OmsiRoadVehicleInst e)
```
The `sender` object as with all the other events refers to the current `OMSIHook` object, the `e` is a reference to the current player vehicle instance.

### _OmsiExited_ Event
Triggered upon OMSI exiting.
```cs
Omsi_OnOmsiExited(object? sender, EventArgs e)
```
The `sender` object as with all the other events refers to the current `OMSIHook` object, the `e` object is not used for this event.

### _OmsiGotD3DContext_ Event
Triggered upon OMSI's Direct3D context being initialised.
```cs
Omsi_OnOmsiGotD3DContext(object? sender, EventArgs e)
```
The `sender` object as with all the other events refers to the current `OMSIHook` object, the `e` object is not used for this event.

### _OmsiLostD3DContext_ Event
Triggered upon OMSI's Direct3D context being lost.
```cs
Omsi_OnOmsiLostD3DContext(object? sender, EventArgs e)
```
The `sender` object as with all the other events refers to the current `OMSIHook` object, the `e` object is not used for this event.
