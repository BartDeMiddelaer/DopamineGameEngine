using Dopamine.Core.Services.EngineServices;

namespace Dopamine.GameFiles.Projects.MetaBalls
{
    public class MetaBallsConfiguration : BaseEngineConfiguration
    {

        public override int WindowWidth { get; set; } = 600;
        public override int WindowHeight { get; set; } = 400;
        public override float WindowScale { get; set; } = 1f;
        public override bool EnableVsync { get; set; } = true;

        public static string[] SplashScreenCategorie { get; set; } = {
            "Rework"
        };
    }
}
