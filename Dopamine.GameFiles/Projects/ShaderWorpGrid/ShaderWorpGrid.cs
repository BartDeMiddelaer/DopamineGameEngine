using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Services.ProjectServices;
using SFML.Graphics;
using SFML.System;

namespace Dopamine.GameFiles.Projects.ShaderWorpGrid
{
    // Shader lessons for Youtube
    // https://www.youtube.com/watch?v=HIvNePu7UEE&list=PL4neAtv21WOmIrTrkNO3xCyrxg4LKkrF7&ab_channel=LewisLepton

    // Info to program from
    // https://github.com/SFML/SFML.Net.git

    public class ShaderWorpGrid : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineFunctionalitys _functionalitys;

        private readonly Shader myShader;
        private readonly Clock clock;

        public ShaderWorpGrid(IRenderer renderer, IEngineConfiguration configuration, IEngineFunctionalitys engineFunctionalitys)
        {
            _functionalitys = engineFunctionalitys;
            _renderer = renderer;

            // get shader file from path still nee to shorten the path name
            Stream shaderFile = 
                new FileStream(
                    _functionalitys.FindPathFileNameInDopamineGameFiles("Shader.frag", "Projects/ShaderWorpGrid"),
                    FileMode.Open);

            // set shader mode to fragment shader
            myShader = new Shader(null, null, shaderFile);
            myShader.SetUniform("resulution", new Vector2f(configuration.WindowWidth, configuration.WindowHeight));

            clock = new Clock();
        }

        public void EventDeclaration(RenderWindow window) { }
        public void GameLoop(RenderWindow window)
        {
            var mPos = _functionalitys.SFML_GetMousePosition();
            // Eddit var in .frag file
            myShader.SetUniform("time", clock.ElapsedTime.AsSeconds());
            
            // Render whit the IRender servis so you dont have to use a image
            _renderer.Draw(window, myShader);
        }
    }
}