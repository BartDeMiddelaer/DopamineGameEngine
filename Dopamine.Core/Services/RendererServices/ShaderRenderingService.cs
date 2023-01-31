using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using SFML.Graphics;
using SFML.System;

namespace Dopamine.Core.Services.RendererServices
{
    public class ShaderRenderingService : IRenderer
    {
        public int PixelCount => Buffer.Length;
        public byte[] Buffer {get; set ;}
        public Texture RenderTexture { get; set; }

        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;

        private Sprite viewport;
        private RenderStates state;
        private Random random;
        private readonly Shader myShader;

        public ShaderRenderingService(IEngineConfiguration configuration, IEngineFunctionalitys functionalitys)
        {
            _configuration = configuration;
            _functionalitys = functionalitys;

            int windowWidth = configuration.WindowWidth;
            int windowHeight = configuration.WindowHeight;

            // (WindowWidth x WindowHeight) x 4 bytes per pixel
            Buffer = new byte[windowWidth * windowHeight * 4];

            RenderTexture = new Texture((uint)windowWidth, (uint)windowHeight);
            viewport = new Sprite(RenderTexture);
            random = new Random();

            Stream shaderFile =
            new FileStream(
                _functionalitys.FindPathFileNameInDopamineCore("RenderShader.frag", "Services/RendererServices"),
                FileMode.Open);

            // set shader mode to fragment shader
            myShader = new Shader(null, null, shaderFile);
            myShader.SetUniform("resulution", new Vector2f(configuration.WindowWidth, configuration.WindowHeight));
        }
        public void Draw(RenderWindow window)
        {
            RenderTexture.Update(Buffer);
            RenderTexture.Smooth = _configuration.SmoothPixelImage;

            myShader.SetUniform("renderTexture", RenderTexture);
            state = new RenderStates(myShader);
            window.Draw(viewport, state);
        }

        public void Draw(RenderWindow window, Shader shader)
        {
            state = new RenderStates(shader);
            window.Draw(viewport, state);
        }

        public void DrawPixel(int x, int y, Color color)
        {
            int index = _functionalitys.SFML_IX(x, y) * 4;

            Buffer[_functionalitys.D1ArryIndexWrapper(index + 0, PixelCount)] = color.R;
            Buffer[_functionalitys.D1ArryIndexWrapper(index + 1, PixelCount)] = color.G;
            Buffer[_functionalitys.D1ArryIndexWrapper(index + 2, PixelCount)] = color.B;
            Buffer[_functionalitys.D1ArryIndexWrapper(index + 3, PixelCount)] = color.A;
        }

        public void StressTest()
        {
            var a = random.Next(1, 255);

            Parallel.For(0, _configuration.WindowWidth * _configuration.WindowHeight, i =>
            {
                int index = i * 4;
                Buffer[index + 0] = (byte)(index / a % 2 == 0 ? 0 : a);
                Buffer[index + 1] = (byte)(index / a % 3 == 0 ? a : 0);
                Buffer[index + 2] = (byte)(index / a % 4 == 0 ? 0 : a);
                Buffer[index + 3] = (byte)a;
            });
        }
    }
}
