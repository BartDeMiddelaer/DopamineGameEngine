using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.ProjectServices;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.ParticleLife
{
    public class ParticleLife : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;

        private Shader myShader;
        private float shift;

        public ParticleLife(IRenderer renderer, IEngineConfiguration configuration, 
            IEngineFunctionalitys engineFunctionalitys)
        {
            _functionalitys = engineFunctionalitys;
            _renderer = renderer;
            _configuration = configuration;
        }

        public void EventDeclaration(RenderWindow window) { }
        public void LoadInProjectAssets()
        {
            // get shader file from path still nee to shorten the path name
            Stream shaderFile =
                new FileStream(
                    _functionalitys.FindPathFileNameInDopamineGameFiles("Shader.frag", "Projects/ParticleLife"),
                    FileMode.Open);

            // set shader mode to fragment shader
            myShader = new Shader(null, null, shaderFile);
        }
        public void GameLoop(RenderWindow window)
        {
            shift += 0.001f;

            // Eddit var in .frag file
            myShader.SetUniform("color_shift", shift);

            // Render whit the IRender servis so you dont have to use a image
            _renderer.Draw(window, myShader);
        }
    }
}
