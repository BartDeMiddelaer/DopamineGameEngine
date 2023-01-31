using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;

namespace Dopamine.GameFiles.Projects.HailstoneNumbers.Entities
{
    public class ThreeXPlusOneGenerator
    {
        public int MaxBatchOfCalculation { get; set; }

        private Action<Index1D, ArrayView<int>, SpecializedValue<int>> kernel;
        private MemoryBuffer1D<int, Stride1D.Dense> gpuOutPut;
        private Context context;
        private Accelerator accelerator;


        public ThreeXPlusOneGenerator(int startMaxBatchOfCalculation)
        {
            context = Context.Create(b => b.Cuda().OpenCL().CPU());

            accelerator = context.CreateCPUAccelerator(0);
            kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<int>, SpecializedValue<int>>(Kernel);

            MaxBatchOfCalculation = startMaxBatchOfCalculation;
            gpuOutPut = accelerator.Allocate1D<int>(MaxBatchOfCalculation);
        }
        public List<int> GenerateSet(int seed)
        {
            gpuOutPut = accelerator.Allocate1D<int>(MaxBatchOfCalculation);
            kernel(MaxBatchOfCalculation, gpuOutPut.View, new SpecializedValue<int>(seed));
            accelerator.Synchronize();

            var numbersToReturn = gpuOutPut.GetAsArray1D().ToList().FindAll(n => n != 1);
            numbersToReturn.Reverse();

            return numbersToReturn;
        }
        public List<List<int>> GenerateMultySet(int numberOfStrands)
        {
            Random random = new Random();
            List<int> randomNumbers = new List<int>();
            List<List<int>> listToReturn = new List<List<int>>();

            for (int i = 0; i < numberOfStrands; i++) randomNumbers.Add(random.Next(0,10000));
            randomNumbers.ForEach(n => listToReturn.Add(GenerateSet(n)));

            return listToReturn;
        }

        static void Kernel(Index1D i, ArrayView<int> output, SpecializedValue<int> seed)
        {
            if (i == 0)
            {
                output[i] = seed;
            }
            else
            {
                if (output[i - 1] != 1)
                {
                    int newOutputElement = output[i - 1] % 2 == 0
                        ? output[i - 1] / 2
                        : (output[i - 1] * 3) + 1;

                    output[i] = newOutputElement % 2 == 0
                        ? -newOutputElement
                        : newOutputElement;
                }
                else
                {
                    output[i] = 1;
                }
            }
        }
    }
}
