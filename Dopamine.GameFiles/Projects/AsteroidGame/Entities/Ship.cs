using Dopamine.Core.ExtensionMethods;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Astroids;
using SFML.Graphics;
using SFML.System;
using System.Numerics;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities
{
    public class Ship
    {
        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IKeyMapping _keyMapping;
        private readonly Random random = new Random();

        public Vector2f Possition { get; set; }
        public Vector2f Velosety { get; set; } = new();
        public float Rotation { get; set; } = 0;
        public Color? ShipColor { get; set; } = Color.Yellow;
        public Color? ShipOutlineColor { get; set; } = Color.Black;
        public Vector2f ShipTipLocation { get; set; } = new();
        public int LifePoints { get; set; } = 3;
        public int NewLifePoints { get; set; }

        private Vertex[] vertexShipShape;
        private Vertex[] vertexShipOutlineShape;

        private AmmoLister ammoLister;
        private float rotationCorection = 90;
        private int shipCircumference = 50;

        public Ship(IEngineConfiguration configuration, IEngineFunctionalitys functionalitys, IKeyMapping keyMapping, AstroidLister astroidLister)
        {
            _configuration = configuration;
            _functionalitys = functionalitys;
            _keyMapping = keyMapping;
            ammoLister = new AmmoLister(_functionalitys, _configuration);

            Possition = FindShipLocation(astroidLister);
            vertexShipShape = ShipVertexInit(ShipColor ?? Color.Transparent);
            vertexShipOutlineShape = ShipVertexInit(ShipOutlineColor ?? Color.Transparent);
            NewLifePoints = LifePoints;
        }
        public void Draw(RenderWindow window)
        {          
            ammoLister.DrawProjectiles(window);
            RenderShip(window);

            if (NewLifePoints != LifePoints) Possition = new(-100, -100);
        }
        private void RenderShip(RenderWindow window)
        {
            Rotation = Rotation > 360 ? Rotation - 360 : Rotation;

            if (ShipColor != null || ShipColor != Color.Transparent)
            {
                vertexShipShape = ShipVertexInit(ShipColor ?? Color.Transparent);
                window.Draw(vertexShipShape, PrimitiveType.TriangleFan);
            }

            if (ShipOutlineColor != null || ShipOutlineColor !=  Color.Transparent)
            { 
                vertexShipOutlineShape = ShipVertexInit(ShipOutlineColor ?? Color.Transparent);
                window.Draw(vertexShipOutlineShape, PrimitiveType.LineStrip);
            }
        }
        private Vertex[] ShipVertexInit(Color color)
        {
            Vertex[] vertices = new Vertex[] {
                new Vertex {
                    Position = _functionalitys.SFML_GetOffset(Possition, shipCircumference, -90 + (Rotation + rotationCorection)),
                    Color = color,
                },
                new Vertex {
                    Position = _functionalitys.SFML_GetOffset(Possition,shipCircumference / 2, 180 + (Rotation + rotationCorection)),
                    Color = color,
                },
                new Vertex {
                    Position = _functionalitys.SFML_GetOffset(Possition, shipCircumference / 5, -90 + (Rotation + rotationCorection)),
                    Color = color,
                },
                new Vertex {
                    Position = _functionalitys.SFML_GetOffset(Possition, shipCircumference / 2, 0 + (Rotation + rotationCorection)),
                    Color = color,
                },
                new Vertex {
                    Position = _functionalitys.SFML_GetOffset(Possition, shipCircumference, -90 + (Rotation + rotationCorection)),
                    Color = color,
                }
            };

            ShipTipLocation = vertices[0].Position;
            return vertices;
        }      
        public void ShipColistionCheak(AstroidLister astroidLister)
        {
            astroidLister.AstroidList.ForEach(a => {            
                if (Vector2.Distance(Possition.ToVec2(), a.Possition.ToVec2()) < (a.Size / 2) + (shipCircumference / 2))
                {
                    // need intersection line to circal
                    NewLifePoints = LifePoints - 1;
                }
            });
        }
        public Vector2f FindShipLocation(AstroidLister astroidLister)
        {

            Vector2f shipLocation = new Vector2f(
                    random.Next(shipCircumference, _configuration.WindowWidth - shipCircumference),
                    random.Next(shipCircumference, _configuration.WindowHeight - shipCircumference));

            bool overlap = astroidLister.AstroidList
                .TrueForAll(a => Vector2.Distance(shipLocation.ToVec2(), a.Possition.ToVec2()) > a.Size);

            while (!overlap)
            {
                shipLocation = new Vector2f(
                    random.Next(shipCircumference, _configuration.WindowWidth - shipCircumference),
                    random.Next(shipCircumference, _configuration.WindowHeight - shipCircumference));

                overlap = astroidLister.AstroidList
                   .TrueForAll(a => Vector2.Distance(shipLocation.ToVec2(), a.Possition.ToVec2()) > a.Size);
            }

            return shipLocation;
        }
        public void ReSpawnShipToMouse()
        {
            Possition = _functionalitys.SFML_GetMousePosition();
            LifePoints = NewLifePoints;
        }
        public void MoveShipToMouse(float maxSpeed)
        {    
            var mousePosition = _functionalitys.SFML_GetMousePosition();
            var angel = _functionalitys.SFML_GetAngel(Possition, mousePosition);
            var directionVector = _functionalitys.SFML_GetMousePositionCartesianCenterPointVariabel(ShipTipLocation);

            Velosety = new Vector2f
            {
                X = _functionalitys.SFML_AddValuePerSec(
                    directionVector.X > maxSpeed ? maxSpeed : directionVector.X < -maxSpeed ? -maxSpeed : directionVector.X),

                Y = _functionalitys.SFML_AddValuePerSec(
                     directionVector.Y > maxSpeed ? maxSpeed : directionVector.Y < -maxSpeed ? -maxSpeed : directionVector.Y)
            };

            Possition = Possition + Velosety;
            Rotation = angel;
        }
        public void ControlerMoveShip(float speed, float maxSpeed)
        {
            if (Possition.X > 0 && 
                Possition.X < _configuration.WindowWidth &&
                Possition.Y > 0 + shipCircumference &&
                Possition.Y < _configuration.WindowHeight)
            {
                var speedToAdd = speed < maxSpeed ? speed : maxSpeed;
                var speedPerSec = _functionalitys.SFML_AddValuePerSec(speedToAdd);
                var moveTo = _functionalitys.SFML_GetOffset(Possition, speedPerSec, Rotation);
                Possition = moveTo;
            }
            else
            {
                if(speed > 0)
                    Possition = _functionalitys.SFML_GetOffset(Possition, -20, Rotation);
                else
                    Possition = _functionalitys.SFML_GetOffset(Possition, 20, Rotation);
            }
        }
    }
}
