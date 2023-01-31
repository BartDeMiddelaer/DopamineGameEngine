using BenchmarkDotNet.Attributes;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Services.EngineServices;
using Dopamine.Core.Services.RendererServices;
using SFML.Graphics;
using SFML.Window;

namespace Dopamine.BenchmarkDotNet.BenchRenderers
{
    [MemoryDiagnoser]
    public class BenchCPUvsGPURenderer
    {
        private readonly IRenderer _cpuRenderer, _gpuRenderer, _shaderRenderer;
        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IWindowStatus _windowStatus;

        private readonly RenderWindow renderWindow;

        public BenchCPUvsGPURenderer()
        {
            _configuration = new DefaultEngineConfigurationService();
            _windowStatus = new WindowStatusService();
            _functionalitys = new EngineFunctionalitysService(_configuration, _windowStatus);

            _cpuRenderer = new CPURendererService(_configuration, _functionalitys);
            _gpuRenderer = new GPURendererService(_configuration, _functionalitys);
            _shaderRenderer = new ShaderRenderingService(_configuration, _functionalitys);

            renderWindow = new RenderWindow(new VideoMode
            {

                Height = (uint)_configuration.WindowHeight,
                Width = (uint)_configuration.WindowWidth,
                BitsPerPixel = _configuration.WindowBitsPerPixel,

            }, "Test GPU vs GPU", _configuration.WindowStyle);

            // Close window for Test
            renderWindow.Close();
        }

        [Benchmark]
        public void BenchCPURendererDrawPixels()
        {
            _cpuRenderer.Buffer = Enumerable.Repeat<byte>(255, _cpuRenderer.Buffer.Length).ToArray();
            _cpuRenderer.Draw(renderWindow);
        }

        [Benchmark]
        public void BenchShaderRendererDrawPixels()
        {
            _cpuRenderer.Buffer = Enumerable.Repeat<byte>(255, _cpuRenderer.Buffer.Length).ToArray();
            _shaderRenderer.Draw(renderWindow);
        }

        [Benchmark]
        public void BenchGPURendererDrawPixels()
        {
            _cpuRenderer.Buffer = Enumerable.Repeat<byte>(255, _cpuRenderer.Buffer.Length).ToArray();
            _gpuRenderer.Draw(renderWindow);
        }
    }
}
