using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Services.ProjectServices;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.TestRendering
{
    public class TestRendering : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        public TestRendering(IRenderer renderer) => _renderer = renderer;

        public void EventDeclaration(RenderWindow window) { }
        public void GameLoop(RenderWindow window)
        {
            _renderer.StressTest();
            _renderer.Draw(window);
        }

    }
}
