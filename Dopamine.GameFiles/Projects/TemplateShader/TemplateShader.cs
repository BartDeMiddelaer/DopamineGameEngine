﻿using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Services.ProjectServices;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.TemplateShader
{
    public class TemplateShader : BaseGameFile, IGameFile
    {
        private readonly IRenderer _renderer;
        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;

        private readonly Shader myShader;
        private float shift;

        public TemplateShader(IRenderer renderer, IEngineConfiguration configuration, IEngineFunctionalitys engineFunctionalitys)
        {
            _functionalitys = engineFunctionalitys;
            _renderer = renderer;
            _configuration = configuration;

            // get shader file from path still nee to shorten the path name
            Stream shaderFile =
                new FileStream(
                    _functionalitys.FindPathFileNameInDopamineGameFiles("SimpelTestShader.frag", "Projects/TemplateShader"),
                    FileMode.Open);

            // set shader mode to fragment shader
            myShader = new Shader(null, null, shaderFile);
        }

        public void EventDeclaration(RenderWindow window) { }
        public void LoadInProjectAssets()
        {

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