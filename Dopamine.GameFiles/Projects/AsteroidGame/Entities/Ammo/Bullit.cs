using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo
{
    public class Bullit
    {
        public Vector2f Possition { get; set; }
        public Vector2f StartPossition { get; set; }
        public bool Hit { get; set; } = false;
        public int Size { get; set; }
        public int Speed { get; set; }
        public float Direction { get; set; }

        private readonly IEngineFunctionalitys _functionalitys;
        
        public Bullit(Vector2f possition,int size, float direction, int speed, IEngineFunctionalitys functionalitys)
        {
            _functionalitys = functionalitys;
            Possition = possition;
            StartPossition = possition;
            Size = size;
            Direction = direction;
            Speed = speed;
        }

        public void DrawBullit(RenderWindow window, Color color, Color outlineColor)
        {
            Possition = _functionalitys.SFML_GetOffset(Possition, _functionalitys.SFML_AddValuePerSec(Speed), Direction);

            CircleShape bullit = new CircleShape();
            bullit.Position = Possition;
            bullit.FillColor = color;
            bullit.OutlineColor = outlineColor;
            bullit.OutlineThickness = 1;
            bullit.Radius = Size / 2;
            bullit.Origin = new Vector2f(Size / 2, Size / 2);

            window.Draw(bullit);
        }
    }
}
