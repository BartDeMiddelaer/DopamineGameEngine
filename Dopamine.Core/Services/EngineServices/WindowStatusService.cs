using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Color = SFML.Graphics.Color;
using Font = SFML.Graphics.Font;

namespace Dopamine.Core.Services.EngineServices
{
    public class WindowStatusService : IWindowStatus
    {
        // Fps counter  -------------------
        public Stopwatch fpsStatusInterval { get; set; } = new Stopwatch();
        public RenderWindow? RenderWindow { get; set; }

        private DateTime lastCheckTime = DateTime.Now;
        private long frameCount = 0;
        private double fps;
        private readonly Text fpsStatus = new Text
        {
            Font = new Font("C:/Windows/Fonts/arial.ttf"),
            Position = new Vector2f(5, 5),
            CharacterSize = 15,
            FillColor = Color.White
        };
        public void UpdateFps()
        {
            double secondsElapsed = (DateTime.Now - lastCheckTime).TotalSeconds;
            long count = Interlocked.Exchange(ref frameCount, 0);

            fps = count / secondsElapsed;
            lastCheckTime = DateTime.Now;

            Interlocked.Increment(ref frameCount);

            if (fpsStatusInterval.Elapsed.TotalMilliseconds > 50)
            {
                fpsStatus.DisplayedString = $" FPS: {Math.Round(fps, 0)} ";
                fpsStatusInterval.Restart();
            }
        }
        public void DrawfpsStatus()
        { 
            var txtWidth = fpsStatus.GetLocalBounds().Width;
            var txtHeight = fpsStatus.GetLocalBounds().Height;

            RectangleShape fpsBackGround = new RectangleShape();
            fpsBackGround.Size = new Vector2f(txtWidth, (txtHeight + txtHeight) - 2);
            fpsBackGround.FillColor = new Color(0,0,0,150);
            fpsBackGround.Position = fpsStatus.Position;
            fpsBackGround.OutlineColor = fpsStatus.FillColor;
            fpsBackGround.OutlineThickness = 1;

            RenderWindow?.Draw(fpsBackGround);
            RenderWindow?.Draw(fpsStatus);
        }
        public double GetFps() => fps;
        // --------------------------------


        // Application Idle ---------------

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        private static extern int PeekMessage(NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);
        public bool IsApplicationIdle() => PeekMessage(new NativeMessage(), IntPtr.Zero, 0, 0, 0) == 0;
        // --------------------------------
    }

}