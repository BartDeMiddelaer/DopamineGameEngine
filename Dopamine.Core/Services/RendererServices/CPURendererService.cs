using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Interfaces.RendererInterfaces;
using SFML.Graphics;

namespace Dopamine.Core.Services.RendererServices
{
    // Rework of
    // https://www.gamedev.net/forums/topic/693835-sfml-and-fast-pixel-drawing-in-c/

    public class CPURendererService : IRenderer
    {
        public int PixelCount { get => Buffer.Length; }
        public byte[] Buffer { get; set; }
        public Texture RenderTexture { get; set; }


        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;

        private Sprite viewport;
        private Random random;
        private RenderStates state;

        public CPURendererService(IEngineConfiguration configuration, IEngineFunctionalitys functionalitys)
        {
            _configuration = configuration;
            _functionalitys = functionalitys;

            int windowWidth = configuration.WindowWidth;
            int windowHeight = configuration.WindowHeight;

            RenderTexture = new Texture((uint)windowWidth, (uint)windowHeight);
            viewport = new Sprite(RenderTexture);

            // (WindowWidth x WindowHeight) x 4 bytes per pixel
            Buffer = new byte[windowWidth * windowHeight * 4];
            random = new Random();
        }
        public void Draw(RenderWindow window)
        {
            RenderTexture.Update(Buffer);
            RenderTexture.Smooth = _configuration.SmoothPixelImage;
            window.Draw(viewport);
        }
        public void Draw(RenderWindow window, Shader shader)
        {
            state = new RenderStates(shader);          
            window.Draw(viewport, state);
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
        public void DrawPixel(int x, int y, Color color)
        {
            int index = _functionalitys.SFML_IX(x, y) * 4;

            Buffer[index + 0] = color.R;
            Buffer[index + 1] = color.G;
            Buffer[index + 2] = color.B;
            Buffer[index + 3] = color.A;
        }    
    }

}
