using BenchmarkDotNet.Attributes;
using Dopamine.BenchmarkDotNet.BenchFluidSimulation.Entities;
using Dopamine.Core.Interfaces.EngineInterfaces;
using Dopamine.Core.Services.EngineServices;

namespace Dopamine.BenchmarkDotNet.BenchFluidSimulation
{
    [MemoryDiagnoser]
    public class BenchFluidDiffuse
    {
        private readonly int Iter, N;
        private readonly float Dt, Diffusion, Viscosity;
        private readonly float[] S, Density, Vx, Vy, Vx0, Vy0;


        private readonly IEngineConfiguration _configuration;
        private readonly IEngineFunctionalitys _functionalitys;
        private readonly IWindowStatus _windowStatus;

        private readonly FluidSettings fluidSettings;
        private readonly FluidPartical[] fluidParticals;

        public BenchFluidDiffuse()
        {
            _configuration = new DefaultEngineConfigurationService();
            _windowStatus = new WindowStatusService();
            _functionalitys = new EngineFunctionalitysService(_configuration, _windowStatus);

            N = _configuration.WindowWidth;
            _configuration.WindowHeight = _configuration.WindowWidth;

            Diffusion = 0.00000000015f;
            Viscosity = 0.000000017f;
            Dt = 0.4f;
            S = new float[N * N];
            Density = new float[N * N];
            Vx = new float[N * N];
            Vy = new float[N * N];
            Vx0 = new float[N * N];
            Vy0 = new float[N * N];
            Iter = 10;

            // TestDiffuseWithEntities
            fluidParticals = Enumerable.Repeat(new FluidPartical(), N * N).ToArray();
            fluidSettings = new FluidSettings
            {

                Diffusion = 0.00000000015f,
                Viscosity = 0.000000017f,
                Dt = 0.4f,
                Iter = 10,
                N = N
            };
        }

        [Benchmark]
        public void TestDiffuseOridginal()
        {
            DiffuseOridgenal(Vx0, Vx, Diffusion, Dt);
        }
        public void DiffuseOridgenal(float[] x, float[] x0, float diff, float dt)
        {
            float a = dt * diff * (N - 2) * (N - 2);
            LinSolveOridgenal(x, x0, a, 1 + 6 * a);
        }
        public void LinSolveOridgenal(float[] x, float[] x0, float a, float c)
        {
            var cRecip = 1.0f / c;

            for (int i = 0; i < Iter; i++)
            {
                Parallel.For(1, N - 1, yD =>
                {
                    for (int xD = 1; xD < N - 1; xD++)
                    {
                        x[_functionalitys.SFML_IX(xD, yD)] =
                          (x0[_functionalitys.SFML_IX(xD, yD)] +
                            a *
                              (x[_functionalitys.SFML_IX(xD + 1, yD)] +
                                x[_functionalitys.SFML_IX(xD - 1, yD)] +
                                x[_functionalitys.SFML_IX(xD, yD + 1)] +
                                x[_functionalitys.SFML_IX(xD, yD - 1)])) *
                          cRecip;
                    }
                });
            }
        }


        [Benchmark]
        public void TestDiffuseWithEntities()
        {
            DiffuseWithEntities(fluidParticals, fluidSettings);
        }
        public void DiffuseWithEntities(FluidPartical[] fluidParticals, FluidSettings fluidSettings)
        {
            float a =
                fluidSettings.Dt * fluidSettings.Diffusion
                * (fluidSettings.N - 2) * (fluidSettings.N - 2);

            LinSolveWithEntities(fluidParticals, a, 1 + 6 * a);
        }
        public void LinSolveWithEntities(FluidPartical[] fluidParticals, float a, float c)
        {
            var cRecip = 1.0f / c;

            for (int i = 0; i < Iter; i++)
            {
                Parallel.For(1, N - 1, yD =>
                {
                    for (int xD = 1; xD < N - 1; xD++)
                    {
                        fluidParticals[_functionalitys.SFML_IX(xD, yD)].Vx =
                          (fluidParticals[_functionalitys.SFML_IX(xD, yD)].Vx0 +
                            a *
                              (fluidParticals[_functionalitys.SFML_IX(xD, yD)].Vx +
                                fluidParticals[_functionalitys.SFML_IX(xD, yD)].Vx +
                                fluidParticals[_functionalitys.SFML_IX(xD, yD)].Vx +
                                fluidParticals[_functionalitys.SFML_IX(xD, yD)].Vx)) *
                          cRecip;
                    }
                });
            }
        }

        [Benchmark]
        public void TestDiffuseNonParallel()
        {
            DiffuseNonParallel(Vx0, Vx, Diffusion, Dt);
        }
        public void DiffuseNonParallel(float[] x, float[] x0, float diff, float dt)
        {
            float a = dt * diff * (N - 2) * (N - 2);
            LinSolveNonParallel(x, x0, a, 1 + 6 * a);
        }
        public void LinSolveNonParallel(float[] x, float[] x0, float a, float c)
        {
            var cRecip = 1.0f / c;

            for (int i = 0; i < Iter; i++) // Iteration dhept of calculation
            {
                for (int yD = 1; yD < N - 1; yD++)
                {
                    for (int xD = 1; xD < N - 1; xD++)
                    {
                        x[_functionalitys.SFML_IX(xD, yD)] =
                            (x0[_functionalitys.SFML_IX(xD, yD)] +
                            a *
                                (x[_functionalitys.SFML_IX(xD + 1, yD)] +
                                x[_functionalitys.SFML_IX(xD - 1, yD)] +
                                x[_functionalitys.SFML_IX(xD, yD + 1)] +
                                x[_functionalitys.SFML_IX(xD, yD - 1)])) *
                            cRecip;
                    }
                }

            }
        }
    }

}
