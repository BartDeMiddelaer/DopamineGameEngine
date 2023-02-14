using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dopamine.Core.Services.ProjectServices.SFMLGameFilesEnums;
using Color = SFML.Graphics.Color;

namespace Dopamine.Core.SFMLTypes
{
    public class InfoPanel
    {
        private readonly IEngineConfiguration _configuration;
        private RectangleShape infoPanelShape = new();
        private Vector2f position, size, maxSize;
        private Color backGroundColor, borderColor, textColor;
        private int width;
        private string name;

        public InfoPanel(PanelOrientation oriantation, IEngineConfiguration configuration, string key)
        {
            _configuration = configuration;
            width = 200;

            size = SetSize(oriantation);
            maxSize = SetMaxSize(oriantation);
            position = SetPosition(oriantation);
            name = SetName(key);

            backGroundColor = new Color(0, 0, 0, 200);
            borderColor = Color.White;
            textColor = Color.White;

            SetInfoPanelShape();
        }
        private Vector2f SetSize(PanelOrientation oriantation)
        {
            int h = oriantation == PanelOrientation.Left ? _configuration.WindowHeight - 40
                : oriantation == PanelOrientation.Right ? _configuration.WindowHeight - 10
                : throw new Exception("Set PanelOrientation");


            int w = oriantation == PanelOrientation.Left ? 20
                : oriantation == PanelOrientation.Right ? -20
                : throw new Exception("Set PanelOrientation");

            return new Vector2f(w, h);
        }
        private Vector2f SetMaxSize(PanelOrientation oriantation)
        {
            int h = _configuration.WindowHeight;
            int w = oriantation == PanelOrientation.Left ? width
                : oriantation == PanelOrientation.Right ? -width
                : throw new Exception("Set PanelOrientation");

            return new Vector2f(w, h);
        }
        private Vector2f SetPosition(PanelOrientation oriantation)
        {
            var leftPosition = new Vector2f(0, 35);
            var rightPosition = new Vector2f(_configuration.WindowWidth, 5);

            return oriantation == PanelOrientation.Left ? leftPosition
                : oriantation == PanelOrientation.Right ? rightPosition
                : throw new Exception("Set PanelOrientation");
        }
        private void SetInfoPanelShape()
        {
            infoPanelShape.Size = size;
            infoPanelShape.Position = position;
            infoPanelShape.FillColor = backGroundColor;
            infoPanelShape.OutlineColor = borderColor;
            infoPanelShape.OutlineThickness = 1;
        }
        private string SetName(string key)
            => !string.IsNullOrEmpty(key) ? key : throw new Exception("Set Key");

        public void Draw(RenderWindow window)
        {
            window.Draw(infoPanelShape);
        }
    }
}
