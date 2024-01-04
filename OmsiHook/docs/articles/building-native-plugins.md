# Building Native Omsi Plugins
Some features of OmsiHook such as remote method calls require the library using OmsiHook to be running as a 
native Omsi plugin. This guide covers some of the basics of building a native Omsi plugin using .NET 6 (and above).
An example plugin is available at 
[https://github.com/space928/Omsi-Extensions/tree/main/OmsiHookPlugin](https://github.com/space928/Omsi-Extensions/tree/main/OmsiHookPlugin).

## Overview of Omsi Plugins
DLLs placed in Omsi's `\plugins\` folder with an accompanying `.opl` file will be loaded on startup as plugins.
The `.opl` file is an INI file containing the name of the DLL to load and the ordered lists of variables 
accessed by the plugin.

#### OPL Files
An example `.opl` file might look like this:
```ini
[dll]
OmsiHookPluginNE.dll

[varlist]
1
door_0

[stringvarlist]
3
INEO_DataTransferVehicleNumber
INEO_DataTransferDelay
INEO_DataTransferPassengerCount

[systemvarlist]
4
Time
Day
Month
Year

[triggers]
1
bus_doorfront0
```

The only required tag in the `.opl` file is the `[dll]` tag, which specifies the name of the DLL to load. 
The other tags in this example specify which Omsi variables and triggers to pass to the plugin. The first
parameter of each of these tags is the number of variables in the list, followed by the name of each variable,
one on each line.

#### DLL Files
Omsi requires that plugins export certain methods, all the required methods must be exported (even if they don't
do anything) for the plugin to be loaded.

Exported methods (C#):
```csharp
[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
public static void PluginStart(IntPtr aOwner) {}

[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
public static void PluginFinalize() {}

[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
public static void AccessVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue)

[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
public static void AccessTrigger(ushort variableIndex, [C99Type("__crt_bool*")] IntPtr triggerScript) { }

[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
public static void AccessStringVariable(ushort variableIndex, [C99Type("char*")] IntPtr firstCharacterAddress, [C99Type("__crt_bool*")] IntPtr writeValue) { }

[UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
public static void AccessSystemVariable(ushort variableIndex, [C99Type("float*")] IntPtr value, [C99Type("__crt_bool*")] IntPtr writeValue) { }
```

In this example, the exported methods have been written in C# with necessary attributes decorating the methods so
that they get exported correctly later when we include the native export library. 

## Setting Up a .NET 6 Project to Use as a Plugin
1. Create a new .NET 6 project with an output type of `Class Library`.
1. Install the [DNNE](https://www.nuget.org/packages/DNNE/) NuGet package.
1. Install the [OmsiHook](https://www.nuget.org/packages/OmsiHook/) NuGet package.
1. Set the platform target to `x86` (Omsi runs exclusively in 32 bit and as such, so must your plugin).
1. Edit your `.csproj` file to include necessary properties for DNNE to build (see the 
[DNNE github](https://github.com/AaronRobinsonMSFT/DNNE) for more details on how to setup native exports):
```xml
<PropertyGroup>
    <!-- Platform target must be set to x86 to be loaded by Omsi -->
    <PlatformTarget>x86</PlatformTarget>
</PropertyGroup>
<PropertyGroup>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <DnneGenerateExports>true</DnneGenerateExports>
    <DnneBuildExports>true</DnneBuildExports>
    <DnneNativeBinaryName></DnneNativeBinaryName>
    <DnneMSBuildLogging>low</DnneMSBuildLogging>
    <DnneAddGeneratedBinaryToProject>false</DnneAddGeneratedBinaryToProject>
    <!-- Enable a workaround for https://github.com/dotnet/sdk/issues/1675 -->
    <DnneWorkAroundSdk1675>true</DnneWorkAroundSdk1675>
    <DnneRuntimeIdentifier>win-x86</DnneRuntimeIdentifier>
    <DnneSelfContained_Experimental>false</DnneSelfContained_Experimental>
</PropertyGroup>
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <!-- Optional -->
    <!-- Copy the built DLLs to the Omsi Plugins folder. NOTE: Make sure the path to Omsi is correct for your machine -->
    <Exec Command="xcopy &quot;$(SolutionDir)Debug\*.dll&quot; &quot;$(OutDir)&quot; /y&#xD;&#xA;xcopy &quot;$(SolutionDir)Release\*.dll&quot; &quot;$(OutDir)&quot; /y&#xD;&#xA;xcopy &quot;$(OutDir)*.dll&quot; &quot;C:\Program Files (x86)\Steam\steamapps\common\OMSI 2\plugins\&quot; /y&#xD;&#xA;xcopy &quot;$(OutDir)*.opl&quot; &quot;C:\Program Files (x86)\Steam\steamapps\common\OMSI 2\plugins\&quot; /y&#xD;&#xA;xcopy &quot;$(OutDir)*.json&quot; &quot;C:\Program Files (x86)\Steam\steamapps\common\OMSI 2\plugins\&quot; /y" />
</Target>
```
6. Ensure that you have the correct dependencies installed on your computer to build the project. See: 
[DNNE Requirements](https://github.com/AaronRobinsonMSFT/DNNE#requirements).
1. Download [Dnne.Attributes.cs](https://github.com/AaronRobinsonMSFT/DNNE/blob/master/test/ExportingAssembly/Dnne.Attributes.cs)
and add it to your project.
1. Create your main plugin file and add all the needed exported methods (see [DLL Files](#dll-files) for an example).
1. Create an `.opl` file (see [OPL Files](#opl-files) for an example); The default name of the exported DLL will be 
`<ProjectName>NE.dll` ie: for a project called `OmsiHookPlugin`, the DLL will be `OmsiHookPluginNE.dll`. This can
be customised using the DNNE project properties.
1. Build your project! If everything was done correctly your Omsi plugins folder should now contain at least:
    * `<ProjectName>.dll`
    * `<ProjectName>NE.dll`
    * `OmsiHook.dll`
    * `<ProjectName>.opl`
    * `<ProjectName>.runtimeconfig.json`
    * If you use remote method calls: `OmsiHookInvoker.dll`
