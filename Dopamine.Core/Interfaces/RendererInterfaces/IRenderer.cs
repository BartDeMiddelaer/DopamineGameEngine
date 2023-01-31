using SFML.Graphics;

namespace Dopamine.Core.Interfaces.RendererInterfaces
{
    public interface IRenderer
    {
        public int PixelCount { get; }
        public byte[] Buffer { get; set; }
        public void Draw(RenderWindow window);
        public void Draw(RenderWindow window, Shader shader);
        public void DrawPixel(int x, int y, Color color);
        public void StressTest();
        public Texture RenderTexture { get; set; }
    }
}
