using SFML.Graphics;

namespace Dopamine.Core.Interfaces.ProjectInterfaces
{
    public interface IGameFile
    {
        public void GameLoop(RenderWindow window);
        public void EventDeclaration(RenderWindow window);
        public void LoadInProjectAssets();
    }
}
