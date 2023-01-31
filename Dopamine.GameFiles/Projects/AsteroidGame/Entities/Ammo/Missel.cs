using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Astroids;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo
{
    public class Missel
    {
        public Vector2f Possition { get; set; }
        public Vector2f StartPossition { get; set; }
        public Astroid? ObjectHit { get; set; } = null;
        public bool Hit { get; set; } = false;
        public int Size { get; set; }
        public bool IsFragment { get; set; }
        public float Direction { get; set; }
        public float Speed { get; set; }

        private readonly IEngineFunctionalitys _functionalitys;

        public Missel(Vector2f possition,int size, float angel, float speed, IEngineFunctionalitys functionalitys)
        {
            _functionalitys = functionalitys;
            Possition = possition;
            StartPossition = possition;
            Size = size;
            Direction = angel;
            Speed = speed;
        }

        public void DrawMissel(RenderWindow window, Color color, Color outlineColor)
        {
            Possition = _functionalitys.SFML_GetOffset(Possition, _functionalitys.SFML_AddValuePerSec(Speed), Direction);

            CircleShape missel = new CircleShape();
            missel.Position = Possition;
            missel.FillColor = color;
            missel.OutlineColor = outlineColor;
            missel.OutlineThickness = 1;
            missel.Radius = Size / 2;
            missel.Origin = new Vector2f(Size / 2, Size / 2);

            window.Draw(missel);
        }
    }
}
