using Dopamine.Core.ExtensionMethods;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities.Astroids
{
    public class AstroidLister
    {
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IEngineConfiguration _configuration;

        public Color AstroidOridginalsColor { get; set; } = Color.Blue;
        public Color AstroidOridginalsOutlineColor { get; set; } = Color.Black;
        public Color AstroidFragmentColor { get; set; } = Color.Green;
        public Color AstroidFragmentOutlineColor { get; set; } = Color.Black;

        public List<Astroid> AstroidList { get; set; } = new();
        public int AstroidOridginals { get; set; } = 5;
        public int OridginalsSpawnDisens { get; set; } = 50;
        public int AstroidMinimumSpeed { get; set; } = 10;
        public int AstroidMaximumSpeed { get; set; } = 100;
        public int AstroindMinimumSize { get; set; } = 80;
        public int AstroindMaximumSize { get; set; } = 200;
        public int AstroindMinimumFragmentSize { get; set; } = 80;
        public int ScoreToAddOnHit { get; set; } = 10;
        public int MaximumFragments { get; set; } = 50;



        private List<Astroid> fragmentsToAdd = new();
        private readonly Random random = new();

        public AstroidLister(IEngineFunctionalitys functionalitys, IEngineConfiguration configuration)
        {
            _functionalitys = functionalitys;
            _configuration = configuration;

            for (int i = 0; i < AstroidOridginals; i++)           
                AstroidList.Add(
                    new Astroid(_functionalitys, _configuration,
                    random.Next(AstroindMinimumSize, AstroindMaximumSize), AstroidList,
                    random.Next(AstroidMinimumSpeed, AstroidMaximumSpeed)));
            
        }
        public void DrawAstroids(RenderWindow window)
        {
            // Oridginals
            AstroidList
                .Where(a => a.IsOridginal)
                .ToList()
                .ForEach(a => a.Draw(window, AstroidOridginalsColor, AstroidOridginalsOutlineColor));

            // Fragments
            AstroidList
                .Where(a => !a.IsOridginal)
                .ToList()
                .ForEach(a => a.Draw(window, AstroidFragmentColor, AstroidFragmentOutlineColor));

            // AstroidColision test
            AstroidList.ForEach(a => a.ColisionToAstroidCheak(AstroidList));
        }
        public void AmmoColisionCheak(AmmoLister ammoLister)
        {
            AstroidList.ForEach(a => a.BullitColisionCheak(ammoLister));
            AstroidList.ForEach(a => a.MisselColisionCheak(ammoLister));
        }
        public void AstoidIsHitAction(UiControls uiControls)
        {
            AstroidList.ForEach(a => {          
                if (a.IsHit) {

                    uiControls.Score += ScoreToAddOnHit;
                    AddFragments(a);
                }                      
            });
            AstroidList.RemoveAll(a => a.IsHit);
            AstroidList.AddRange(fragmentsToAdd);
            fragmentsToAdd.Clear();
        }
        public void AddOridginals(Vector2f shipPossition)
        {
            List<Astroid> astroidToAdd = new();
            int count = AstroidList.Count(a => a.IsOridginal);

            var newAstroid = new Astroid(_functionalitys, _configuration,
                             random.Next(AstroindMinimumSize, AstroindMaximumSize), AstroidList,
                             random.Next(AstroidMinimumSpeed, AstroidMaximumSpeed));

            newAstroid.Possition = NewAstroidOridginalLocation(shipPossition, newAstroid.Size);

            if (count < AstroidOridginals) astroidToAdd.Add(newAstroid);
            AstroidList.AddRange(astroidToAdd);
        }
        public Vec3[] GetAstroidOridginalsForShader()
        {
            List<Vec3> toReturn = new();
            var oridginals = AstroidList.Where(a => a.IsOridginal).ToList();

            oridginals.ForEach(a => 
            toReturn.Add(new Vec3(
                a.Possition.X, 
                a.Possition.Y, 
                a.Size)));

            return toReturn.ToArray();
        }
        public Vec3[] GetAstroidFragmentsForShader()
        {
            List<Vec3> toReturn = new();
            var fragments = AstroidList.Where(a => !a.IsOridginal).ToList();

            fragments.ForEach(a =>
            toReturn.Add(new Vec3(
                a.Possition.X,
                a.Possition.Y,
                a.Size)));

            return toReturn.ToArray();
        }
        private void AddFragments(Astroid astroid)
        {
            int fragmentCount = AstroidList.Count(a => !a.IsOridginal);

            if (astroid.Size > AstroindMinimumFragmentSize && fragmentCount < MaximumFragments)
            {
                for (int i = 0; i < 360; i += 90)
                {
                    fragmentsToAdd.Add(
                        new Astroid(
                            _functionalitys, _configuration,
                            (int)(astroid.Size / 3),
                            astroid.Possition, astroid.Direction + i, astroid.Speed / 1.2f));
                }
            }
        }
        private Vector2f NewAstroidOridginalLocation(Vector2f shipPossition, float astroidSize)
        {
           var location = new Vector2f(
                   random.Next(0 + (int)astroidSize / 2, _configuration.WindowWidth - (int)astroidSize / 2),
                   random.Next(0 + (int)astroidSize / 2, _configuration.WindowHeight - (int)astroidSize / 2));


            while (Vector2.Distance(shipPossition.ToVec2(), location.ToVec2()) < astroidSize + OridginalsSpawnDisens)
            {
                location = new Vector2f(
                    random.Next(0 + (int)astroidSize / 2, _configuration.WindowWidth - (int)astroidSize / 2),
                    random.Next(0 + (int)astroidSize / 2, _configuration.WindowHeight - (int)astroidSize / 2));
            }

            return location;
        }

    }
}
