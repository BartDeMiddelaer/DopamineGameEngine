using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.Interfaces.ProjectInterfaces;
using Dopamine.Core.Services.ProjectServices;
using ILGPU;
using ILGPU.Runtime;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.TestILGPU
{
    public class TestILGPU : BaseGameFile, IGameFile, IDisposable
    {
        private readonly IRenderer _renderer;
        private readonly ILGPU.Context context;
        private readonly Accelerator accelerator;
        private byte[] cudaData;

        private readonly MemoryBuffer1D<byte, Stride1D.Dense> dataOut;
        private readonly Action<Index1D, ArrayView<byte>> loadedKernel;

        public TestILGPU(IRenderer renderer)
        {
            _renderer = renderer;

            context = ILGPU.Context.Create(
               builder =>
               {
                   builder.Math(MathMode.Fast);
                   builder.AllAccelerators();
               });

            accelerator = context.GetPreferredDevice(preferCPU: false)
                                 .CreateAccelerator(context);

            cudaData = new byte[_renderer.PixelCount];
            dataOut = accelerator.Allocate1D<byte>(_renderer.PixelCount);
            loadedKernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<byte>>(ColorKernal);

            dataOut.MemSetToZero();
        }
        static void ColorKernal(Index1D index, ArrayView<byte> dataOutput)
        {
            byte color;

            color = (byte)(index.X % 0 == 0 ? 200 : 250);
            color = (byte)(index.X % 1 == 0 ? 255 : 0);
            color = (byte)(index.X % 2 == 0 ? 0 : 255);
            color = (byte)(index.X % 3 == 0 ? 0 : 255);

            dataOutput[index] = color;
        }

        public void EventDeclaration(RenderWindow window) { }
        public void GameLoop(RenderWindow window)
        {
            // indexing for the gpu loops
            int indexing = _renderer.PixelCount;

            // finish compiling and tell the accelerator to start computing the kernel
            loadedKernel(indexing, dataOut.View);

            // wait for the accelerator to be finished with whatever it's doing
            // in this case it just waits for the kernel to finish.
            accelerator.Synchronize();

            // moved output data from the GPU to the CPU
            cudaData = dataOut.GetAsArray1D();

            _renderer.Buffer = cudaData;
            _renderer.Draw(window);
        }
        public void Dispose()
        {
            context.Dispose();
            accelerator.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
