using Dopamine.Core.Services.EngineServices;
using SFML.Graphics;
using SFML.Window;

namespace Dopamine.GameFiles.Projects.ParticleLife
{
    public class ParticleLifeConfiguration : BaseEngineConfiguration
    {
        public override uint WindowBitsPerPixel { get; set; } = 32;
        public override int WindowWidth { get; set; } = 800;
        public override int WindowHeight { get; set; } = 600;
        public override float WindowScale { get; set; } = 1f;
        public override bool EnableVsync { get; set; } = false;
        public override Styles WindowStyle { get; set; } = Styles.Default;
        public override Color BackGroundColor { get; set; } = new Color(238, 238, 238);
        public override bool SmoothPixelImage { get; set; } = true;

        public static string[] SplashScreenCategorie { get; set; } = {
            "Particles"
        };
    }
}
