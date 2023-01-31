using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.Window;

namespace Dopamine.Core.Services.EngineServices
{
    public abstract class BaseEngineConfiguration : IEngineConfiguration
    {
        // bitsPerPixel represents the bit depth, also know as the color depth
        // Usually you would use a value of 32 here to have good rendering
        public virtual uint WindowBitsPerPixel { get; set; } = 32;

        // Set dymentions of the window
        public virtual int WindowWidth { get; set; } = 800;
        public virtual int WindowHeight { get; set; } = 600;

        // set the scale of the window
        public virtual float WindowScale { get; set; } = 1f;

        // Set the main name of the window
        public virtual string Titel { get; set; } = "Dopamine GE";

        // Enable or disabel Vsync
        public virtual bool EnableVsync { get; set; } = false;

        // Style of the window
        public virtual Styles WindowStyle { get; set; } = Styles.Default;

        // Set the background color
        public virtual Color BackGroundColor { get; set; } = new Color(238, 238, 238);

        // Blurs out the pixels if true "a sort of AA"
        public virtual bool SmoothPixelImage { get; set; } = true;

        // Set max fps if null FramerateLimit is disabeld
        public virtual uint? FramerateLimit { get; set; } = null;
    }
}
