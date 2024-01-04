using System;
using System.Threading;
using FFMediaToolkit;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using OmsiHook;

namespace VideoDemo
{
    class Program
    {
        const string VIDEO_PATH = @"https://adam.mathieson.dev/sample.mp4";
        const string FFMPEG_PATH = @"ffmpeg-shared\bin";
        const int ST_INDEX = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("#=#=#=#=#=# OMSIHook Video Playback Demo #=#=#=#=#=#");

            OmsiHook.OmsiHook omsi = new();
            omsi.AttachToOMSI().Wait();

            DXTextureManager textureManager = new();
            textureManager.Init(omsi);

            var markerBlink = false;
            // Configure the FFmpeg path to accelerate the video decode
            FFmpegLoader.FFmpegPath = FFMPEG_PATH;
            var videoFile = MediaFile.Open(VIDEO_PATH, new MediaOptions()
            {
                VideoPixelFormat = ImagePixelFormat.Bgra32
            });
            var frameBuffer = new byte[videoFile.Video.Info.FrameSize.Width * videoFile.Video.Info.FrameSize.Height * 4];
            var FPS = videoFile.Video.Info.AvgFrameRate;
            while (true)
            {
                if (!textureManager.IsReady)
                {
                    Console.SetCursorPosition(0, 1);
                    if (textureManager.CreateTexture(videoFile.Video.Info, ST_INDEX))
                    {
                        Console.WriteLine("💻 Successfully created a texture... " + (markerBlink ? "🔴" : "⚫"));
                    }
                    else
                    {
                        Console.WriteLine("💻 Trying to create texture... " + (markerBlink ? "🔴" : "⚫"));
                    }
                } else
                {
                    int counter = 0;
                    while (videoFile.Video.TryGetNextFrame(frameBuffer.AsSpan()))
                    {
                        textureManager.UpdateTexture(frameBuffer);
                        Console.SetCursorPosition(0, 2);
                        Console.WriteLine("💿 Video Playing... " + (markerBlink ? "🔴" : "⚫"));
                        if (counter % 10 == 0)
                            markerBlink = !markerBlink;
                        counter++;
                        Thread.Sleep((int)Math.Round((1.0 / FPS) * 1000));
                    }
                    return;
                }
                markerBlink = !markerBlink;
                Thread.Sleep(20);
            }
        }
    }
}
