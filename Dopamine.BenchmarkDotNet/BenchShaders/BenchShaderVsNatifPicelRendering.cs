using BenchmarkDotNet.Attributes;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.EngineServices;
using Dopamine.Core.Services.RendererServices;
using SFML.Graphics;
using SFML.Window;

namespace Dopamine.BenchmarkDotNet.BenchShaders
{
    [MemoryDiagnoser]
    public class BenchShaderVsNatifPicelRendering
    {
        IRenderer _cpuRenderer;
        IEngineConfiguration _configuration;
        IEngineFunctionalitys _functionalitys;
        IWindowStatus _windowStatus;

        RenderWindow renderWindow;
        Shader renderShader;

        float shift;

        public BenchShaderVsNatifPicelRendering()
        {
            _configuration = new DefaultEngineConfigurationService();
            _windowStatus = new WindowStatusService();
            _functionalitys = new EngineFunctionalitysService(_configuration, _windowStatus);

            _cpuRenderer = new CPURendererService(_configuration, _functionalitys);
            renderWindow = new RenderWindow(new VideoMode
            {

                Height = (uint)_configuration.WindowHeight,
                Width = (uint)_configuration.WindowWidth,
                BitsPerPixel = _configuration.WindowBitsPerPixel,

            }, "Test GPU vs GPU", _configuration.WindowStyle);

            Stream shaderFile =
                new FileStream(_functionalitys.FindPathFileNameInDopamineBenchmarkDotNet("SimpelTestShader.frag", "BenchShaders"),
                FileMode.Open);

            // set shader mode to fragment shader
            renderShader = new Shader(null, null, shaderFile);

            renderWindow.Close();
        }

        [Benchmark]
        public void CPURendering_Shader_ON()
        {
            shift += 0.001f;

            // Eddit var in .frag file
            renderShader.SetUniform("color_shift", shift);

            // Render whit the IRender servis so you dont have to use a image
            _cpuRenderer.Draw(renderWindow, renderShader);
        }

        [Benchmark]
        public void CPURendering_Shader_OFF() => _cpuRenderer.Draw(renderWindow);
      
    }
}
