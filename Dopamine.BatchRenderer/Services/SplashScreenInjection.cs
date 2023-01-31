using Autofac;
using Dopamine.BatchRenderer.SplashScreenComponents;
using Dopamine.BatchRenderer.SplashScreenComponents.Interfaces;
using Dopamine.BatchRenderer.SplashScreenComponents.Services;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Services.EngineServices;

namespace Dopamine.BatchRenderer.Services
{
    public class SplashScreenInjection
    {
        public string Project { get; set; } = string.Empty;

        public void Inject()
        {
            var splachScreenContainer = BuildContainer();

            // Ceate the startup Splashscreen to pick a project
            // SplashScreen splashScreen = new (new SplashScreenFunctionalitiesService());

            using (var scope = splachScreenContainer.BeginLifetimeScope())
            {
                var splashScreen = scope.Resolve<SplashScreen>();
                splashScreen.ShowDialog();

                // Stops here untill splashScreen.Close() is trigerd "trigers inside splashScreen with the flowPannel"

                Project = splashScreen.GameToStart;
            }
        }
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<SplashScreenFunctionalitiesService>()
                .As<ISplashScreenFunctionalities>();

            // Services for drawing fuctunalety and calculations and file navigation
            builder.RegisterType<EngineFunctionalitysService>().As<IEngineFunctionalitys>();

            // Needs to fireup the EngineFunctionalitysService
            builder.RegisterType<DefaultEngineConfigurationService>().As<IEngineConfiguration>();
            builder.RegisterType<WindowStatusService>().As<IWindowStatus>();

            builder.RegisterType<SplashScreen>();
            return builder.Build();
        }
    }
}
