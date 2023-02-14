using Dopamine.Core.Services.RendererServices;
using Dopamine.BatchRenderer.Services;
using Dopamine.BatchRenderer.SplashScreenComponents;
using Autofac;

namespace Dopamine.BatchRenderer
{
    // -------------------------------------------------------------------------------
    // 
    // Start op the program is here
    //
    // -------------------------------------------------------------------------------
    // Alle you need to do is name the folder and the Gamefile the same
    // And add a Configuration file names GamefileNameConfiguration
    // Preview
    //
    // Make it in Dopamine.GameFiles Domain
    //
    // Folder = SomeGame
    //      Gamefile    = SomeGame.cs
    //      ConfigFile  = SomeGameConfiguration.cs
    //
    // And the pannel + autofac will do the rest
    // to add a button to the flowPanel to start the gameFile
    // -------------------------------------------------------------------------------
    // Autofac help
    // https://www.youtube.com/watch?v=9Pha-pKUqWA&ab_channel=AndreaAngella

    internal static class BatchRenderer
    {
        [STAThread]    
        static void Main()
        {
            //SplashScreenInjection splashScreenInjection = new();
            //splashScreenInjection.Inject();

            ProjectInjection<CPURendererService> projectInjection = new();
            //projectInjection.Inject(splashScreenInjection.Project);
            projectInjection.Inject("ParticleLife");
        }
    }
}