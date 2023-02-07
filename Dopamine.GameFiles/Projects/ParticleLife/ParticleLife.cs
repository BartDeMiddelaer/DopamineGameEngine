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

        public ParticleLife(IRenderer renderer, IEngineConfiguration configuration, IEngineFunctionalitys engineFunctionalitys)
        {
            _functionalitys = engineFunctionalitys;
            _renderer = renderer;
            _configuration = configuration;
        }

        public void EventDeclaration(RenderWindow window) { }
        public void LoadInProjectAssets()
        {

        }
        public void GameLoop(RenderWindow window)
        {
            _renderer.Draw(window);
        }
    }
}
