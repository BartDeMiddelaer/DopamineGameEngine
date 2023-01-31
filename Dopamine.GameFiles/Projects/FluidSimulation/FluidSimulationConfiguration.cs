using Dopamine.Core.Services.EngineServices;

namespace Dopamine.GameFiles.Projects.FluidSimulation
{
    public class FluidSimulationConfiguration : BaseEngineConfiguration
    {
        public override int WindowWidth { get; set; } = 350;
        public override int WindowHeight { get; set; } = 350;
        public override float WindowScale { get; set; } = 1.5f;
        public override bool EnableVsync { get; set; } = true;


        public static string[] SplashScreenCategorie { get; set; } = {
            "Rework"
        };       
    }
}
