# Video Playback Demo

This article provides a basic understanding to a basic C# .NET example leveraging the OMSIHook library. This demo is a more advanced 
demo using FFmpeg's library to decode video files then pipe the frame data into OMSI's ScriptTextures.

_This article is in direct relation to the Sample Project available [here](https://github.com/space928/Omsi-Extensions/tree/main/_OmsiHookExamples/Video_Demo)._

![youtube.com/embed 1](https://www.youtube.com/embed/61sMzpSCn0M)

## First Steps
Before trying the project, the FFmpeg binaries need to be compiled - they should be compiled for **Win32** with shared libraries. 
Prebuilt binaries are available [here](https://rwijnsma.home.xs4all.nl/files/ffmpeg/). A video file is also required to 
demonstrate it, MPEG-4/H.264 format is recommended however many formats are compatible with FFmpeg.

Once the required resources are acquired, some configuration is needed; At the top of `Program.cs` the consts are defined as follows:
```cs
const string VIDEO_PATH = @"sample.mp4"; // Path/URL to the video file
const string FFMPEG_PATH = @"ffmpeg-shared\bin"; // Path to your FFmpeg binaries
const int ST_INDEX = 0; // The index of the Script Texture to replace
```

## What's Going On
The main program is split in two main classes `Program` and `DXTextureManager`.
 - `Program` is the entry point to the demo, it initialises OMSIHook and FFmpeg, and loads the video file.
 - `DXTextureManager` handles initialising textures in OMSI, assigning them to ScriptTextures and updating them.

