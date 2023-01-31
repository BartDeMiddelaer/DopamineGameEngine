using Dopamine.Core.ExtensionMethods;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities.Astroids
{
    public class Astroid
    {
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IEngineConfiguration _configuration;

        public float Size { get; set; }
        public float Speed { get; set; }
        public float Direction { get; set; }
        public bool IsHit { get; set; } = false;
        public bool IsOridginal { get; set; }     
        public Vector2f Possition { get; set; }

        private readonly Random random = new Random();

        public Astroid(IEngineFunctionalitys functionalitys, IEngineConfiguration configuration, int size, List<Astroid> astroidCompanions, float speed)
        {
            _functionalitys = functionalitys;
            _configuration = configuration;
            Size = size;
            Speed = speed;

            Possition = FindNewAstroidLocation(astroidCompanions);
            Direction = random.Next(0, 360);
            IsOridginal = true;
        }
        public Astroid(IEngineFunctionalitys functionalitys, IEngineConfiguration configuration, int size, Vector2f possition, float direction, float speed)
        {
            _functionalitys = functionalitys;
            _configuration = configuration;
            Size = size;
            Speed = speed;

            Possition = possition;
            Direction = direction;

            IsOridginal = false;
        }
        public void Draw(RenderWindow window, Color color, Color outlineColor)
        {
            CircleShape shape = new CircleShape();
            shape.Position = Possition;
            shape.OutlineColor = outlineColor;
            shape.FillColor = color;
            shape.OutlineThickness = 1;
            shape.Radius = Size / 2;
            shape.Origin = new Vector2f(Size / 2, Size / 2);

            MoveAstroid();
            window.Draw(shape);
        }
        private void MoveAstroid()
        {
            float speed = 0;
            speed = _functionalitys.SFML_AddValuePerSec(Speed);

            Possition = _functionalitys.SFML_GetOffset(Possition, speed, Direction);
            Direction = Direction > 360 ? Direction - 360 : Direction;
        }

        public void MisselColisionCheak(AmmoLister ammoLister)
        {
            ammoLister.MisselList.ForEach(m => {

                if (Vector2.Distance(Possition.ToVec2(), m.Possition.ToVec2()) < (Size + m.Size) / 2)
                {
                    IsHit = true;
                    m.Hit = true;
                    m.ObjectHit = this;
                }
            });
        }

        public void BullitColisionCheak(AmmoLister ammoLister)
        {
            ammoLister.BullitList.ForEach(b => { 

                if (Vector2.Distance(Possition.ToVec2(), b.Possition.ToVec2()) < (Size + b.Size) / 2)
                {
                    IsHit = true;
                    b.Hit = true;
                }
            });
        }
        public void ColisionToAstroidCheak(List<Astroid> astroidCompanions)
        {
            if (Possition.X < 0 + Size / 2 || Possition.X > _configuration.WindowWidth - Size / 2 ||
                Possition.Y < 0 + Size / 2|| Possition.Y > _configuration.WindowHeight - Size / 2)
            {
                Direction += 45;
            }

            astroidCompanions.Where(a => a.Possition != Possition).ToList().ForEach(astroid => {

                if (Vector2.Distance(astroid.Possition.ToVec2(), Possition.ToVec2()) < (astroid.Size + Size) /2)
                {
                    var distensToTravelAwaye = 
                        Vector2.Distance(astroid.Possition.ToVec2(), Possition.ToVec2()) - (astroid.Size + Size) / 2;
                    
                    var newDirection = _functionalitys.SFML_GetAngel(astroid.Possition, Possition);
                    Direction += newDirection;

                    Possition = _functionalitys.SFML_GetOffset(Possition, distensToTravelAwaye, Direction);
                }
            });
        }
        private Vector2f FindNewAstroidLocation(List<Astroid> astroidCompanions)
        {

            Vector2f location = new Vector2f(
                random.Next(0 + (int)Size / 2, _configuration.WindowWidth - (int)Size / 2),
                random.Next(0 + (int)Size / 2, _configuration.WindowHeight - (int)Size / 2));


            bool overlap = astroidCompanions
                .TrueForAll(ac => Vector2.Distance(location.ToVec2(), ac.Possition.ToVec2()) > (Size / 2 + ac.Size / 2));

            while (!overlap)
            {
                location = new Vector2f(
                    random.Next(0 + (int)Size / 2, _configuration.WindowWidth - (int)Size / 2),
                    random.Next(0 + (int)Size / 2, _configuration.WindowHeight - (int)Size / 2));

                overlap = astroidCompanions
                .TrueForAll(ac => Vector2.Distance(location.ToVec2(), ac.Possition.ToVec2()) > (Size / 2 + ac.Size / 2));
            }

            return location;
        }
    }
}
