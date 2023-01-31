using Autofac;
using Dopamine.BatchRenderer.SplashScreenComponents;
using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;
using Dopamine.BatchRenderer.SplashScreenComponents.Services;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Services.EngineServices;
using System.Reflection;

namespace Dopamine.BatchRenderer.Services
{
    public class LoadScreenInjection
    {
        public string Project { get; set; } = string.Empty;
        
        public void Inject(GameLoopLogic? loopLogic, string gameFile)
        {          
            var loadScreenContainer = BuildContainer(gameFile);

            // Ceate the startup Splashscreen to pick a project
            // SplashScreen splashScreen = new (new SplashScreenFunctionalitiesService());

            using (var scope = loadScreenContainer.BeginLifetimeScope())
            {
                var loadScreen = scope.Resolve<LoadScreen>();

                while (loadScreen.IsLoadingAssits(loopLogic))
                {
                    loadScreen.Show();
                }

                loadScreen.Close();
                loopLogic?.RunGameCycle();
            }
        }
        private static IContainer BuildContainer(string gameFile)
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<SplashScreenFunctionalitiesService>()
                .As<ISplashScreenFunctionalities>();

            // Services for drawing fuctunalety and calculations and file navigation
            builder.RegisterType<EngineFunctionalitysService>().As<IEngineFunctionalitys>();

            // Load domain
            Assembly domain = Assembly.Load("Dopamine.GameFiles");

            // Configuration class out of Assembly
            var configurationAssembly =
                domain.GetType($"Dopamine.GameFiles.Projects.{gameFile}.{gameFile}Configuration")
                ?? typeof(DefaultEngineConfigurationService);

            // Redgister Configuration
            builder.RegisterAssemblyTypes(configurationAssembly.Assembly)
               .Where(c => c.Name == $"{gameFile}Configuration")
               .As<IEngineConfiguration>()
               .SingleInstance();

            builder.RegisterType<WindowStatusService>().As<IWindowStatus>();
            builder.RegisterType<LoadScreen>();
            return builder.Build();
        }
    }
}
