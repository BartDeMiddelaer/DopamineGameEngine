using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Image = SFML.Graphics.Image;
using Dopamine.BatchRenderer.SplashScreenComponents;

namespace Dopamine.BatchRenderer
{
    public class GameLoopLogic
    {
        // _configuration is to set the chosen configuration to the GameLoopLogic

        private readonly IGameFile _gameFile;
        private readonly IEngineConfiguration _configuration;
        private readonly IWindowStatus _windowStatus;
        private readonly IEngineFunctionalitys _functionalitys;
        private Task? loadInGameAssits = null;

        public GameLoopLogic(IEngineConfiguration configuration, IWindowStatus windowStatus ,
                              IGameFile gameFile, IEngineFunctionalitys functionalitys)
        {
            _windowStatus = windowStatus;
            _configuration = configuration;
            _functionalitys = functionalitys;
            _gameFile = gameFile;
           
            CreateRenderWindow();
            ConfigurRenderWindow();
            SetIco();
        }
        public void RunGameCycle() 
        {
            // Game loop
            if (_windowStatus.RenderWindow != null)
            {
                while (_windowStatus.RenderWindow.IsOpen)
                {
                    // Update Framerate
                    _windowStatus.UpdateFps();

                    // Clear DrawWindow
                    _windowStatus.RenderWindow.Clear(_configuration.BackGroundColor);

                    // Event Dispaching
                    _windowStatus.RenderWindow.DispatchEvents();

                    // Run Game loop lodgic
                    _gameFile.GameLoop(_windowStatus.RenderWindow);
                    _windowStatus.DrawfpsStatus();

                    // Show Renderwindow
                    _windowStatus.RenderWindow.Display();
                }
            }
            else throw new ArgumentNullException(nameof(_configuration));
        }
        public Task LoadAssets()
        {
            if (loadInGameAssits == null)
            {
                loadInGameAssits = new Task(() => _gameFile.LoadInProjectAssets());              
                loadInGameAssits.Start();
            }
            return loadInGameAssits;
        }
        private void CreateRenderWindow()
        {
            // Make and open SFML OpenGl window
            _windowStatus.RenderWindow = new RenderWindow(new VideoMode
            {
                Height = (uint)_configuration.WindowHeight,
                Width = (uint)_configuration.WindowWidth,
                BitsPerPixel = _configuration.WindowBitsPerPixel,
            },
            $"{_configuration.Titel} {_gameFile}     Enabled Vsync -> {_configuration.EnableVsync}",
            _configuration.WindowStyle);

        }
        private void ConfigurRenderWindow()
        {
            if (_windowStatus.RenderWindow != null)
            {
                // Turn Vsync on or off
                _windowStatus.RenderWindow.SetVerticalSyncEnabled(_configuration.EnableVsync);

                // Set fps limit
                if(_configuration.FramerateLimit != null)
                _windowStatus.RenderWindow.SetFramerateLimit((uint)_configuration.FramerateLimit);

                // Events          
                _gameFile.EventDeclaration(_windowStatus.RenderWindow);
                _windowStatus.RenderWindow.MouseMoved += _functionalitys.SFML_MousePositionEvent;
                _windowStatus.RenderWindow.Closed +=
                    (sender, e) => _windowStatus.RenderWindow.Close();

                // Start Fps Timer
                _windowStatus.fpsStatusInterval.Start();

                // Set Window scaling
                _windowStatus.RenderWindow.Size =
                    new Vector2u((uint)(_configuration.WindowWidth * _configuration.WindowScale),
                                 (uint)(_configuration.WindowHeight * _configuration.WindowScale));
            }
            else throw new ArgumentNullException(nameof(_configuration));

        }
        private void SetIco()
        {
            var icoPath = _functionalitys.FindPathFileNameInDopamineBatchRenderer("dopamine.png", "Image");
            Image image = new Image(icoPath);
            _windowStatus?.RenderWindow?.SetIcon(image.Size.X, image.Size.Y, image.Pixels);
        }
    }
}
