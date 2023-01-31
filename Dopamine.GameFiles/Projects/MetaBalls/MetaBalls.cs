using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.NewTypes;
using Dopamine.Core.Services.ProjectServices;
using Dopamine.GameFiles.Projects.MetaBalls.Entities;
using SFML.Graphics;
using System.Numerics;

namespace Dopamine.GameFiles.Projects.MetaBalls
{
    // https://www.youtube.com/watch?v=ccYLb7cLB1I&t=804s&ab_channel=TheCodingTrain

    public class MetaBalls : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineConfiguration _configuration;
        private readonly List<MetaBall> metaBalles;
        private readonly HSLColor hslColor = new();


        public MetaBalls(IRenderer renderer, IEngineConfiguration configuration)
        {
            _renderer = renderer;
            _configuration = configuration;
            metaBalles = new List<MetaBall>();
            Enumerable.Repeat(metaBalles, 10)
                .ToList()
                .ForEach(mb => mb.Add(new MetaBall(50,20, configuration)));
       
        }
        public void EventDeclaration(RenderWindow window) {}
        public void LoadInProjectAssets()
        {

        }
        public void GameLoop(RenderWindow window)
        {
            
            FillIsoSurface();
            metaBalles.ForEach(mb => mb.ColisionMetaBallsToCheak = metaBalles);
            metaBalles.ForEach(mb => mb.Update());

            _renderer.Draw(window);
            //DrawOutlineMetaballs(window);
        }
        private void FillIsoSurface()
        {
            for (int x = 0; x < _configuration.WindowWidth; x++)
            {
                for (int y = 0; y < _configuration.WindowHeight; y++)
                {
                    float col = 0;

                    metaBalles.ForEach(mb => { 
                    
                        Vector2 a = new (x, y);
                        Vector2 b = new (mb.Position.X, mb.Position.Y);

                        float d = Vector2.Distance(b, a);
                        col += 100 * (mb.Radius / d);
                                 
                    });

                    hslColor.SetRGB(255, 0, 0);
                    hslColor.Hue = col;
                    _renderer.DrawPixel(x, y, hslColor);
                }

            }
        }
    }
}
