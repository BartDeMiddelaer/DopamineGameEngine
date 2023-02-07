using Dopamine.Core.Services.EngineServices;
using SFML.Graphics;
using SFML.Window;

namespace Dopamine.GameFiles.Projects.AsteroidGame
{
    public class AsteroidGameConfiguration : BaseEngineConfiguration
    {
        public override uint WindowBitsPerPixel { get; set; } = 32;
        public override int WindowWidth { get; set; } = 1450;
        public override int WindowHeight { get; set; } = 900;
        public override float WindowScale { get; set; } = 1f;
        public override bool EnableVsync { get; set; } = false;
        public override Styles WindowStyle { get; set; } = Styles.Close;
        public override Color BackGroundColor { get; set; } = Color.Black;
        public override bool SmoothPixelImage { get; set; } = true;

        public static string[] SplashScreenCategorie { get; set; } = {
            "Game",
            "Shader"
        };
    }
}
