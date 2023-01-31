using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using SFML.System;
using System.Numerics;
using Dopamine.Core.ExtensionMethods;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities.Ammo
{
    public class AmmoLister
    {
        public int MaxBullits { get; set; } = 7;
        public int MaxMissels { get; set; } = 2;
        public int MaxBullitTravil { get; set; } = 500;
        public int MaxMisselTravil { get; set; } = 1000;
        public int BulletSize { get; set; } = 20;
        public int MisselSize { get; set; } = 20;
        public int MisseFragmentlSize { get; set; } = 10;

        public Color BullitsColor { get; set; } = Color.Red;
        public Color BullitsOutlineColor { get; set; } = Color.White;
        public Color MisselsColor { get; set; } = Color.Red;
        public Color MisselsOutlineColor { get; set; } = Color.White;
        public Color MisselsFragmentColor { get; set; } = Color.Cyan;
        public Color MisselsFragmentOutlineColor { get; set; } = Color.White;

        public List<Bullit> BullitList { get; set; } = new();
        public List<Missel> MisselList { get; set; } = new();


        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IEngineConfiguration _configuration;

        private SoundControls soundControls;
        private List<Missel> misselFragmentsToAdd = new();

        public AmmoLister(IEngineFunctionalitys functionalitys, IEngineConfiguration configuration)
        {
            _functionalitys = functionalitys;
            _configuration = configuration;
            soundControls = new SoundControls(_functionalitys);
        }
        public void ReleasedBullit(Vector2f possition, float angel, int speed)
        {
            BullitList.Add(new Bullit(possition, BulletSize, angel, speed, _functionalitys));
        }
        public void ReleasedMissel(Vector2f possition, float angel, int speed)
        {
            MisselList.Add(new Missel(possition, MisselSize, angel, speed, _functionalitys));
        }
        public void DrawProjectiles(RenderWindow window)
        {
            BullitList.RemoveAll(b => Vector2.Distance(b.StartPossition.ToVec2(), b.Possition.ToVec2()) > MaxBullitTravil);
            BullitList.ForEach(bullit => bullit.DrawBullit(window,BullitsColor,BullitsOutlineColor));

            MisselList.RemoveAll(m => Vector2.Distance(m.StartPossition.ToVec2(), m.Possition.ToVec2()) > MaxMisselTravil);
            
            // Draw missels
            MisselList
                .Where(m => !m.IsFragment)
                .ToList()
                .ForEach(missel => missel.DrawMissel(window,MisselsColor,MisselsOutlineColor));

            // Draw missel fragments
            MisselList
                .Where(m => m.IsFragment)
                .ToList()
                .ForEach(missel => missel.DrawMissel(window,MisselsFragmentColor,MisselsFragmentOutlineColor));

            BullitList.RemoveAll(b => b.Hit == true);
            ScatterMissel();
        }

        private void ScatterMissel()
        {
            MisselList.ForEach(m => AddMisselFragments(m));
            MisselList.RemoveAll(m => m.Hit == true);
            MisselList.AddRange(misselFragmentsToAdd);
            misselFragmentsToAdd.Clear();
        }
        private void AddMisselFragments(Missel missel)
        {
            if (missel.Hit && missel.IsFragment == false)
            {
                for (int i = 0; i < 5; i++)
                {
                    var frafmentDirection =
                        _functionalitys.SFML_GetAngel(
                            missel.Possition,
                            missel.ObjectHit?.Possition ?? new()) + 180;

                    var frafmentPosition =
                        _functionalitys.SFML_GetOffset(missel.Possition, 10, frafmentDirection);

                    Missel newMissel = 
                        new Missel(frafmentPosition, MisseFragmentlSize, frafmentDirection + (10 * i),
                                    missel.Speed, _functionalitys);
                    
                    newMissel.IsFragment = true;
                    misselFragmentsToAdd.Add(newMissel);
                }
            }
        }
        public string GetInfo()
        {
            return $"Bullits in use = {MaxBullits} / {BullitList.Count} " +
                    $"Missels in use = {MaxMissels} / {MisselList.Count(m => !m.IsFragment)} " +
                    $"Missels Fragfment = {MisselList.Count(m => m.IsFragment)} ";
        }
        public void ShootBullit(int speed, Vector2f Origin, Vector2f TipLocation)
        {
            if (BullitList.Count < MaxBullits)
            {
                var angel = _functionalitys.SFML_GetAngel(Origin, TipLocation);
                ReleasedBullit(TipLocation, angel, speed);
                soundControls.PlayBullitSound();
            }
        }
        public void ShootMissel(int speed, Vector2f Origin, Vector2f TipLocation)
        {
            if (MisselList.Count(m => !m.IsFragment) < MaxMissels)
            {
                var angel = _functionalitys.SFML_GetAngel(Origin, TipLocation);
                ReleasedMissel(TipLocation, angel, speed);
                soundControls.PlayMissileSound();
            }

        }
    }
}
