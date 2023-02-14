namespace Dopamine.GameFiles.Panels
{
    public static class Panels
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            ApplicationConfiguration.Initialize();
        }
        public static void OpenParticleLifeConfigPanel()
        {
            Application.Run(new ParticleLifeConfigPanel());
        }
    }
}