# Basic Trigger & Sound Trigger Example

This article offers a foundational exploration of a C# .NET example that utilizes the OMSIHook library. The focus of this example is on activating existing OMSI Mouse Events (triggers) and initiating sound playback (sound triggers).

_For a hands-on experience, you can access the associated Sample Project [here](https://github.com/space928/Omsi-Extensions/tree/main/_OmsiHookExamples/TriggersSample)._

## Mouse Triggers

To manipulate the state of any trigger within OMSI, a straightforward remote call to the `SetTrigger()` method is employed. This method allows you to alter the boolean state of a specific trigger identified by its name. The code snippet below illustrates this process:

```cs
playerVehicle.SetTrigger("bus_doorfront0", true);
```

## Sound Triggers

In a similar fashion, sound manipulation is achieved through a remote call to OMSI's `SoundTrigger()` method. This method facilitates the playback of any sound by referencing its file name and the designated point specified in the `\sound.cfg\` file. The following code snippet demonstrates this operation:

```cs
playerVehicle.SoundTrigger("ev_IBIS_Ansagen", @"..\..\MAN_NL_NG\Sound\Matrix_Ziel.wav");
```

These examples showcase the simplicity and effectiveness of leveraging the OMSIHook library to interact with mouse events and sound triggers within OMSI, enhancing the extensibility and functionality of your C# .NET applications.