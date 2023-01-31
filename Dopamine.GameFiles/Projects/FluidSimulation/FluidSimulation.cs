using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.ProjectServices;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.FluidSimulation
{
    // https://mikeash.com/pyblog/fluid-simulation-for-dummies.html

    public class FluidSimulation : BaseGameFile, IGameFile
    {
        private readonly Fluid fluid;
        private readonly Random rand = new();

        private int oldX = 1, oldY = 1;
        private int diferentsInX;
        private int diferentsInY;

        private readonly IEngineFunctionalitys _engineFunctionalitys;
        private readonly IRenderer _renderer;


        public FluidSimulation(IEngineConfiguration enginePropertys, IEngineFunctionalitys engineFunctionalitys, IRenderer renderer)
        {
            _engineFunctionalitys = engineFunctionalitys;
            _renderer = renderer;

            fluid = new Fluid(enginePropertys.WindowWidth,
            0.4f, // Difusion
            0.00000000015f, // How fast the smoke disepares diffusion 
            0.000000017f, // viscosety
            6); // Iterations To calculate
        }

        public void EventDeclaration(RenderWindow window)
        {

        }
        public void GameLoop(RenderWindow window)
        {
            Demo();
            fluid.Step();
            fluid.Render(_renderer);

            _renderer.Draw(window);
        }

        private void Demo()
        {
            MouseDispursion(0.5f);
            BurningCandel(rand.Next(10, 255), 10);
        }
        private void BurningCandel(float densitty, int maxFlimeHight)
        {
            fluid.AddDensity(fluid.Size / 2, fluid.Size - 2, densitty);
            fluid.AddVelocity(fluid.Size / 2, fluid.Size - 2, 0, rand.Next(-maxFlimeHight, 0));
        }
        private void MouseDispursion(float divideIntensetyBy)
        {
            var mousePosition = _engineFunctionalitys.SFML_GetMousePosition();

            diferentsInX = (int)mousePosition.X - oldX;
            diferentsInY = (int)mousePosition.Y - oldY;

            oldX = (int)mousePosition.X;
            oldY = (int)mousePosition.Y;

            fluid.AddVelocity(oldX, oldY, diferentsInX / divideIntensetyBy, diferentsInY / divideIntensetyBy);

        }

    }
}
