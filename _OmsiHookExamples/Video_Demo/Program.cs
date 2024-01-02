using System;
using System.Threading;
using FFMediaToolkit;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using OmsiHook;

namespace Video_Demo
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
            DXTextureManager texture_manager = new();
            texture_manager.Init(omsi);
            var marker = false;
            // Configure the FFmpeg path to accelerate the video decode
            FFmpegLoader.FFmpegPath = FFMPEG_PATH;
            var video_file = MediaFile.Open(VIDEO_PATH, new MediaOptions()
            {
                VideoPixelFormat = ImagePixelFormat.Bgra32
            });
            var frameBuffer = new byte[video_file.Video.Info.FrameSize.Width * video_file.Video.Info.FrameSize.Height * 4];
            var FPS = video_file.Video.Info.AvgFrameRate;
            while (true)
            {
                if (!texture_manager.IsReady)
                {
                    Console.SetCursorPosition(0, 1);
                    if (texture_manager.CreateTexture(video_file.Video.Info, ST_INDEX))
                    {
                        Console.WriteLine("💻 Successfully created a texture... " + (marker ? "🔴" : "⚫"));
                    }
                    else
                    {
                        Console.WriteLine("💻 Trying to create texture... " + (marker ? "🔴" : "⚫"));
                    }
                }
                if (texture_manager.IsReady)
                {
                    int counter = 0;
                    while (video_file.Video.TryGetNextFrame(frameBuffer.AsSpan()))
                    {
                        texture_manager.UpdateTexture(frameBuffer);
                        Console.SetCursorPosition(0, 2);
                        Console.WriteLine("💿 Video Playing... " + (marker ? "🔴" : "⚫"));
                        if (counter % 10 == 0)
                            marker = !marker;
                        counter++;
                        Thread.Sleep((int)Math.Round((1.0 / FPS) * 1000));
                    }
                    return;
                }
                marker = !marker;
                Thread.Sleep(20);
            }
        }
    }
}
