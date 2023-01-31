using Dopamine.Core.Interfaces.EngineInterfaces;
using System.Numerics;

namespace Dopamine.GameFiles.Projects.MetaBalls.Entities
{
    public class MetaBall
    {
        public Vector2 Position { get; set; }
        public Vector2 Velosety { get; set; }
        public float Radius { get; set; }
        public List<MetaBall> ColisionMetaBallsToCheak { get; set; } = new();

        private readonly IEngineConfiguration _engineConfiguration;


        private readonly Random random = new();
        public MetaBall(int maxRadius, int maxVelosety, IEngineConfiguration engineConfiguration)
        {
            _engineConfiguration = engineConfiguration;
            Radius = random.Next(20, maxRadius);

            Position = 
                new Vector2(
                    random.Next(maxRadius, _engineConfiguration.WindowWidth), 
                    random.Next(maxRadius, _engineConfiguration.WindowHeight));

            Velosety = 
                new Vector2(
                    random.Next(1, maxVelosety),
                    random.Next(1, maxVelosety));

        }
        public void Update()
        {
            Position = Vector2.Add(Position, Velosety);

            if (Position.X > _engineConfiguration.WindowWidth || Position.X < 0)
            
                Velosety = new Vector2(-Velosety.X, Velosety.Y);

            if (Position.Y > _engineConfiguration.WindowHeight || Position.Y < 0)
                Velosety = new Vector2(Velosety.X, -Velosety.Y);

        }            
    }
}
