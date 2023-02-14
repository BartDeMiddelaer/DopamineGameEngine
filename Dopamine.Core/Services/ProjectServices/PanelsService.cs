using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.SFMLTypes;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dopamine.Core.Services.ProjectServices.SFMLGameFilesEnums;

namespace Dopamine.Core.Services.ProjectServices
{
    public class PanelsService : IPanels
    {
        private readonly IEngineConfiguration _configuration;
        private List<InfoPanel> infoPanels = new();

        public PanelsService(IEngineConfiguration configuration) 
            => _configuration = configuration;
        
        public void AddPanel(PanelOrientation oriantation, string key)
        {
            // to do add divisions

            infoPanels.Add(new InfoPanel(oriantation, _configuration, key));
        }

        public void Draw(RenderWindow window)
        {
            RoundedRectangleShape rectangleShape = new RoundedRectangleShape(new FloatRect(100, 100, 100, 100), 10, Color.Black, Color.White, 1);
            window.Draw(rectangleShape);

            infoPanels.ForEach(p => p.Draw(window));
        }
    }
}
