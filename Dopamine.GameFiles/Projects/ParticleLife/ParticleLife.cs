using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.ProjectServices;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using SFML.Graphics;
using Dopamine.Core.SFMLTypes;
using static Dopamine.Core.Services.ProjectServices.SFMLGameFilesEnums;
using SFML.System;

namespace Dopamine.GameFiles.Projects.ParticleLife
{
    public class ParticleLife : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IWindowStatus _windowStatus;
        private readonly IPanels _panels;

        private Shader myShader;
        private float shift;

        public ParticleLife(IRenderer renderer, IEngineConfiguration configuration, 
            IEngineFunctionalitys engineFunctionalitys, IWindowStatus windowStatus, IPanels panels)
        {
            _functionalitys = engineFunctionalitys;
            _renderer = renderer;
            _configuration = configuration;
            _windowStatus = windowStatus;
            _panels = panels;
        }

        public void EventDeclaration(RenderWindow window) { }
        public void LoadInProjectAssets()
        {
            // get shader file from path still nee to shorten the path name
            Stream shaderFile =
                new FileStream(
                    _functionalitys.FindPathFileNameInDopamineGameFiles("Shader.frag", "Projects/ParticleLife"),
                    FileMode.Open);

            // set shader mode to fragment shader
            myShader = new Shader(null, null, shaderFile);
            myShader.SetUniform("resulution", new Vector2f(_configuration.WindowWidth, _configuration.WindowHeight));

            _panels.AddPanel(PanelOrientation.Left, "Settings");
            //_panels.AddPanel(PanelOrientation.Left, "Settings_2");
        }
        public void GameLoop(RenderWindow window)
        {
            shift += 0.001f;

            // Eddit var in .frag file
            myShader.SetUniform("color_shift", shift);

            // Render whit the IRender servis so you dont have to use a image
            _renderer.Draw(window, myShader);
        }
    }
}
