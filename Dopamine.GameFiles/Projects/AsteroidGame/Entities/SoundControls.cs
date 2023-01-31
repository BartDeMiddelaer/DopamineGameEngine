using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopamine.GameFiles.Projects.AsteroidGame.Entities
{
    public class SoundControls
    {
        private readonly IEngineFunctionalitys _functionalitys;
        private SoundBuffer? bullitBuffer, MissileBuffer;
        private Sound? bullit, Missile;

        public SoundControls(IEngineFunctionalitys functionalitys)
        {
            _functionalitys = functionalitys;
            CreateSound();
        }
        private void CreateSound()
        {
            string bullitPath = _functionalitys.FindPathFileNameInDopamineGameFiles("BullitShot.ogg", "Projects/AsteroidGame/Sounds");
            bullitBuffer = new SoundBuffer(bullitPath);
            bullit = new Sound(bullitBuffer);
            bullit.Volume = 10;

            string missilePath = _functionalitys.FindPathFileNameInDopamineGameFiles("MissileBlast.ogg", "Projects/AsteroidGame/Sounds");
            MissileBuffer = new SoundBuffer(missilePath);
            Missile = new Sound(MissileBuffer);
            Missile.Volume = 10;
        }
        public void PlayBullitSound() => bullit?.Play();
        public void PlayMissileSound() => Missile?.Play();
    }
}
