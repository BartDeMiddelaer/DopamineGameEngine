using Autofac;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.EngineServices;
using Dopamine.Core.Services.ProjectServices;
using System.Reflection;

namespace Dopamine.BatchRenderer.Services
{
    public class ProjectInjection<Renderer> where Renderer : IRenderer
    {
        public void Inject(string gameToRun)
        {
            if (gameToRun != string.Empty)
            {
                // if splashScreen.GameToStart != string.Empty it will start the dependesy injection with autofac
                var gameContainer = BuildContainer(gameToRun);
                var gameContainerScope = gameContainer.BeginLifetimeScope();       
                var GameLoop = gameContainerScope.Resolve<GameLoopLogic>();

                var loadAssets = GameLoop.LoadAssets();
                loadAssets.Start();
                loadAssets.Wait();

                GameLoop.RunGameCycle();               
            }
            else Application.Exit();
        }
        private void SetGameFileEndConfigFile(ContainerBuilder builder, string gameFileName)
        {
            // https://stackoverflow.com/questions/1825147/type-gettypenamespace-a-b-classname-returns-null
            // https://stackoverflow.com/questions/26838880/autofac-register-assembly-types

            // Load domain
            Assembly domain = Assembly.Load("Dopamine.GameFiles");

            // Configuration class out of Assembly
            var configurationAssembly =
                domain.GetType($"Dopamine.GameFiles.Projects.{gameFileName}.{gameFileName}Configuration")
                ?? typeof(DefaultEngineConfigurationService);

            // Redgister Configuration
            builder.RegisterAssemblyTypes(configurationAssembly.Assembly)
               .Where(c => c.Name == $"{gameFileName}Configuration")
               .As<IEngineConfiguration>()
               .SingleInstance();

            // GameFile class out of Assembly
            var gameFileAsambly =
                domain.GetType($"Dopamine.GameFiles.Projects.{gameFileName}.{gameFileName}")
                ?? typeof(BaseGameFile);

            // Redgister Gamefile
            builder.RegisterAssemblyTypes(gameFileAsambly.Assembly)
              .Where(c => c.Name == gameFileName)
              .As<IGameFile>();
        }
        private IContainer BuildContainer(string gameToRun)
        {
            var builder = new ContainerBuilder();

            // gameToRun info come's from the buttons in the ProjectNavigation
            // "dataflow =
            //      -> Button pressed adds Project name in ProjectNavigation.GameFile
            //      -> SplashScreen.GameToStart
            //      -> StartupGameFileAndDependencys(gameToRun)
            //      -> BuildContainer(gameToRun)"
            //      -> SetGameFileEndConfigFile(builder,-->gameToRun<--)

            SetGameFileEndConfigFile(builder, gameToRun);

            // Services to regulate Fps and status of the window
            builder
                .RegisterType<WindowStatusService>()
                .As<IWindowStatus>()
                .SingleInstance();

            builder
                .RegisterType<DualSenseKeyMappingService>()
                .As<IKeyMapping>()
                .SingleInstance();

            // Services for drawing fuctunalety and calculations
            builder
                .RegisterType<EngineFunctionalitysService>()
                .As<IEngineFunctionalitys>()
                .SingleInstance();

            builder
                .RegisterType<PanelsService>()
                .As<IPanels>()
                .SingleInstance();

            // Type of renderer you want
            builder
                .RegisterType<Renderer>() // <-- set type of rendering gpu or cpu
                .As<IRenderer>()
                .SingleInstance();

            // Register the GameLoopLogic to run the Gameloop
            builder.RegisterType<GameLoopLogic>();

            return builder.Build();
        }
    }
}
