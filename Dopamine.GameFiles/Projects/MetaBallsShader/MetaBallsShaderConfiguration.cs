using Dopamine.Core.Services.EngineServices;
using SFML.Window;

namespace Dopamine.GameFiles.Projects.MetaBallsShader
{
    public class MetaBallsShaderConfiguration : BaseEngineConfiguration
    {
        public override int WindowWidth { get; set; } = 1260;
        public override int WindowHeight { get; set; } = 850;
        public override bool EnableVsync { get; set; } = true;
        public override Styles WindowStyle { get; set; } = Styles.Close;

        public static string[] SplashScreenCategorie { get; set; } = {            
             "Shader"
        };
    }
}
