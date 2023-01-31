using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Interfaces.EngineInterfaces;
using SFML.Graphics;
using ILGPU;
using ILGPU.Runtime;

namespace Dopamine.Core.Services.RendererServices
{
    public class GPURendererService : IRenderer, IDisposable
    {
        public byte[] Buffer { get; set; }
        public int PixelCount { get; }
        public Texture RenderTexture { get; set; }

        private Sprite viewport;

        readonly private Context context;
        readonly private Accelerator accelerator;

        readonly private IEngineFunctionalitys _engineFunctionalitys;
        readonly private IEngineConfiguration _enginePropertys;

        private MemoryBuffer1D<byte, Stride1D.Dense> bufferGpu;
        private MemoryBuffer1D<byte, Stride1D.Dense> pixelsGPUOutput;

        private Action<Index1D, ArrayView<byte>, ArrayView<byte>> kernel;

        private Random random = new();

        public GPURendererService(IEngineConfiguration enginePropertysService, IEngineFunctionalitys engineFunctionalitys)
        {
            _engineFunctionalitys = engineFunctionalitys;
            _enginePropertys = enginePropertysService;

            Buffer = new byte[enginePropertysService.WindowWidth * enginePropertysService.WindowHeight * 4];
            PixelCount = Buffer.Length;

            RenderTexture = new Texture((uint)enginePropertysService.WindowWidth, (uint)enginePropertysService.WindowHeight);
            viewport = new Sprite(RenderTexture);

            // cuda 
            context = Context.Create(
              builder => {
                  builder.Math(MathMode.Fast);
                  builder.AllAccelerators();
              });

            accelerator = context.GetPreferredDevice(preferCPU: false)
                                 .CreateAccelerator(context);

            bufferGpu = accelerator.Allocate1D<byte>(Buffer);
            pixelsGPUOutput = accelerator.Allocate1D<byte>(Buffer.Length);

            kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>, ArrayView<byte>>(Kernal);
        }
        public void Draw(RenderWindow window)
        {
            // CUDA
            bufferGpu = accelerator.Allocate1D(Buffer);
            kernel(Buffer.Length, bufferGpu.View, pixelsGPUOutput.View);
            accelerator.Synchronize();

            // SFML
            RenderTexture.Update(pixelsGPUOutput.GetAsArray1D());
            RenderTexture.Smooth = _enginePropertys.SmoothPixelImage;
            window.Draw(viewport);
        }

        public void Draw(RenderWindow window, Shader shader)
        {
            // CUDA
            bufferGpu = accelerator.Allocate1D(Buffer);
            kernel(Buffer.Length, bufferGpu.View, pixelsGPUOutput.View);
            accelerator.Synchronize();

            // SFML
            RenderTexture.Update(pixelsGPUOutput.GetAsArray1D());
            RenderTexture.Smooth = _enginePropertys.SmoothPixelImage;

            var state = new RenderStates(shader);
            window.Draw(viewport, state);
        }

        public void DrawPixel(int x, int y, Color color)
        {
            int index = _engineFunctionalitys.SFML_IX(x, y) * 4;
            Buffer[index + 0] = color.R;
            Buffer[index + 1] = color.G;
            Buffer[index + 2] = color.B;
            Buffer[index + 3] = color.A;
        }

        public void StressTest()
        {
            var a = random.Next(1, 255);
            Parallel.For(0, _enginePropertys.WindowWidth * _enginePropertys.WindowHeight, i =>
            {
                int index = i * 4;
                Buffer[index + 0] = (byte)(index / a % 2 == 0 ? 0 : a);
                Buffer[index + 1] = (byte)(index / a % 3 == 0 ? a : 0);
                Buffer[index + 2] = (byte)(index / a % 4 == 0 ? 0 : a);
                Buffer[index + 3] = (byte)a;
            });
        }

        public static void Kernal(Index1D index, ArrayView<byte> buffer, ArrayView<byte>bufferOutput)
        {
            bufferOutput[index] = buffer[index];
        }

        public void Dispose()
        {
            context.Dispose();
            accelerator.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
