using SFML.Graphics;
using System.Diagnostics;

namespace Dopamine.Core.Interfaces.EngineInterfaces
{
    public interface IWindowStatus
    {
        public RenderWindow? RenderWindow { get; set; }
        public Stopwatch fpsStatusInterval { get; set; }
        public void UpdateFps();
        public void DrawfpsStatus();
        public double GetFps();
        public bool IsApplicationIdle();
    }
}
