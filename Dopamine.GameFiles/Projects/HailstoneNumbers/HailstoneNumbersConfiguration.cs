using Dopamine.Core.Services.EngineServices;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.HailstoneNumbers
{
    public class HailstoneNumbersConfiguration : BaseEngineConfiguration
    {
        public override int WindowWidth { get; set; } = 1200;
        public override int WindowHeight { get; set; } = 900;
        public override float WindowScale { get; set; } = 1f;
        public override Color BackGroundColor { get; set; } = new Color(0, 0, 0);
        //public override uint? FramerateLimit { get; set; } = 10;

        public static string[] SplashScreenCategorie { get; set; } = {
            "Cuda",
            "Presentation",
            "Done Myself"
        };
    }
}
