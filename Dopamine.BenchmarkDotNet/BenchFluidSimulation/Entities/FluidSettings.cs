
namespace Dopamine.BenchmarkDotNet.BenchFluidSimulation.Entities
{
    public class FluidSettings
    {
        public int N { get; set; }
        public int Iter { get; set; }
        public float Dt { get; set; }
        public float Diffusion { get; set; }
        public float Viscosity { get; set; }
    }
}
