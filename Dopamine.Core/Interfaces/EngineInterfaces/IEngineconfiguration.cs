using SFML.Graphics;
using SFML.Window;

namespace Dopamine.Core.Interfaces.EngineInterfaces
{
    public interface IEngineConfiguration
    {
        public uint WindowBitsPerPixel { get; set; }
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        public float WindowScale { get; set; }
        public string Titel { get; set; }
        public bool EnableVsync { get; set; }
        public Styles WindowStyle { get; set; }
        public Color BackGroundColor { get; set; }
        public bool SmoothPixelImage { get; set; }
        public uint? FramerateLimit { get; set; }
    }
}
