using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Audio;
using SFML.System;

namespace Dopamine.GameFiles.Projects.MetaBallsShader.Entities
{
    public class SoundControls
    {
        private readonly IEngineFunctionalitys _functionalitys;

        private readonly Clock clock = new();
        private Sound ping, pop, pianoBackground, statUpdate;
        private SoundBuffer upPingBuffer, popBuffer, pianoBackgroundBuffer, statUpdateBuffer;

        // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        #pragma warning disable CS8618
        public SoundControls(IEngineFunctionalitys functionalitys)
        {
            _functionalitys = functionalitys;
            CreateSound();
            CreateBackGroundSound();
        }     
        #pragma warning restore CS8618

        private void CreateSound()
        {
            string upPingSoundPath = _functionalitys.FindPathFileNameInDopamineGameFiles("Ping.ogg", "Projects/MetaBallsShader/Sounds");
            upPingBuffer = new SoundBuffer(upPingSoundPath);
            ping = new Sound(upPingBuffer);
            ping.Volume = 50;

            string popSoundPath = _functionalitys.FindPathFileNameInDopamineGameFiles("Pop.ogg", "Projects/MetaBallsShader/Sounds");
            popBuffer = new SoundBuffer(popSoundPath);
            pop = new Sound(popBuffer);
            pop.Volume = 20;

            string statUpdatePath = _functionalitys.FindPathFileNameInDopamineGameFiles("StatUpdate.ogg", "Projects/MetaBallsShader/Sounds");
            statUpdateBuffer = new SoundBuffer(statUpdatePath);
            statUpdate = new Sound(statUpdateBuffer);
            statUpdate.Volume = 20;
        }
        private void CreateBackGroundSound()
        {
            string pianoBackgroundSoundPath = _functionalitys.FindPathFileNameInDopamineGameFiles("PianoBackground.ogg", "Projects/MetaBallsShader/Sounds");
            pianoBackgroundBuffer = new SoundBuffer(pianoBackgroundSoundPath);
            pianoBackground = new Sound(pianoBackgroundBuffer);

            pianoBackground.Play();
            pianoBackground.Loop = true;
        }
        public void FideInBackgroundSound()
        {
            pianoBackground.Volume = clock.ElapsedTime.AsSeconds() * 5;
        }
        public void PlayPopSound() => pop.Play();
        public void PlayPingSound() => ping.Play();
        public void PlayStatUpdateSound() => statUpdate.Play();
    }
}
