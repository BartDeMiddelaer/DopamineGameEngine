using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dopamine.Core.Services.ProjectServices.SFMLGameFilesEnums;

namespace Dopamine.Core.Interfaces.ProjectInterfaces
{
    public interface IPanels
    {
        void Draw(RenderWindow window);
        void AddPanel(PanelOrientation oriantation, string key);
    }
}
