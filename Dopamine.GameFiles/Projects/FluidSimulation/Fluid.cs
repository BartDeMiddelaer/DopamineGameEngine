using Dopamine.Core.Interfaces.RendererInterfaces;
using Dopamine.Core.NewTypes;
using SFML.Graphics;

namespace Dopamine.GameFiles.Projects.FluidSimulation
{
    public class Fluid
    {
        public int Size { get; }
        public int Iter { get; }

        public float Dt { get; }
        public float Diffusion { get; }
        public float Viscosity { get; }

        public float[] S { get; }
        public float[] Density { get; }
        public float[] Vx { get; }
        public float[] Vy { get; }
        public float[] Vx0 { get; }
        public float[] Vy0 { get; }

        private readonly HSLColor hslColor = new();


        public Fluid(int n, float dt, float diffusion, float viscosity, int iter)
        {
            Size = n;

            Diffusion = diffusion;
            Viscosity = viscosity;
            Dt = dt;
            S = new float[n * n];
            Density = new float[n * n];
            Vx = new float[n * n];
            Vy = new float[n * n];
            Vx0 = new float[n * n];
            Vy0 = new float[n * n];
            Iter = iter;
        }

        public void AddDensity(int x, int y, float amount)
        {
            if (x < Size && x > 0 && y < Size && y > 0)
                Density[IX(x, y)] += amount;
        }
        public void AddVelocity(int x, int y, float amountX, float amountY)
        {
            if (x < Size && x > 0 && y < Size && y > 0)
            {
                Vx[IX(x, y)] += amountX;
                Vy[IX(x, y)] += amountY;
            }
        }

        public int IX(int x, int y) => x + y * Size;

        private void Diffuse(int b, float[] x, float[] x0, float diff, float dt)
        {
            float a = dt * diff * (Size - 2) * (Size - 2);
            LinSolve(b, x, x0, a, 1 + 6 * a);
        }

        private void LinSolve(int b, float[] x, float[] x0, float a, float c)
        {
            var cRecip = 1.0f / c;

            //for (int i = 0; i < Iter; i++)
            //{
            //    x = cudaServise.LinSolveCudaAccelration(x, x0, a, c);
            //    SetBnd(b, x);
            //}

            for (int i = 0; i < Iter; i++)
            {
                Parallel.For(1, Size - 1, yD =>
                {
                    for (int xD = 1; xD < Size - 1; xD++)
                    {
                        x[IX(xD, yD)] =
                          (x0[IX(xD, yD)] +
                            a *
                              (x[IX(xD + 1, yD)] +
                                x[IX(xD - 1, yD)] +
                                x[IX(xD, yD + 1)] +
                                x[IX(xD, yD - 1)])) *
                          cRecip;
                    }
                });
                SetBnd(b, x);
            }
        }

        private void Project(float[] velocX, float[] velocY, float[] p, float[] div)
        {
            Parallel.For(1, Size - 1, yD =>
            {
                for (int xD = 1; xD < Size - 1; xD++)
                {
                    div[IX(xD, yD)] = -0.5f * (
                                velocX[IX(xD + 1, yD)]
                            - velocX[IX(xD - 1, yD)]
                            + velocY[IX(xD, yD + 1)]
                            - velocY[IX(xD, yD - 1)]

                        ) / Size;
                    p[IX(xD, yD)] = 0;
                }
            });

            SetBnd(0, div);
            SetBnd(0, p);
            LinSolve(0, p, div, 1, 6);

            Parallel.For(1, Size - 1, yD =>
            {
                for (int xD = 1; xD < Size - 1; xD++)
                {
                    velocX[IX(xD, yD)] -= 0.5f * (p[IX(xD + 1, yD)] - p[IX(xD - 1, yD)]) * Size;
                    velocY[IX(xD, yD)] -= 0.5f * (p[IX(xD, yD + 1)] - p[IX(xD, yD - 1)]) * Size;
                }
            });

            SetBnd(1, velocX);
            SetBnd(2, velocY);
        }

        private void Advect(int b, float[] d, float[] d0, float[] velocX, float[] velocY, float dt)
        {

            float i0, i1, j0, j1;

            float dtx = dt * (Size - 2);
            float dty = dt * (Size - 2);

            float s0, s1, t0, t1;
            float tmp1, tmp2, x, y;

            float Nfloat = Size - 2;
            float ifloat, jfloat;
            int xD, yD;

            for (yD = 1, jfloat = 1; yD < Size - 1; yD++, jfloat++)
            {
                for (xD = 1, ifloat = 1; xD < Size - 1; xD++, ifloat++)
                {
                    tmp1 = dtx * velocX[IX(xD, yD)];
                    tmp2 = dty * velocY[IX(xD, yD)];
                    x = ifloat - tmp1;
                    y = jfloat - tmp2;

                    if (x < 0.5f) x = 0.5f;
                    if (x > Nfloat + 0.5f) x = Nfloat + 0.5f;
                    i0 = (float)Math.Floor(x);
                    i1 = i0 + 1.0f;
                    if (y < 0.5) y = 0.5f;
                    if (y > Nfloat + 0.5f) y = Nfloat + 0.5f;
                    j0 = (float)Math.Floor(y);
                    j1 = j0 + 1.0f;

                    s1 = x - i0;
                    s0 = 1.0f - s1;
                    t1 = y - j0;
                    t0 = 1.0f - t1;

                    int i0i = (int)i0;
                    int i1i = (int)i1;
                    int j0i = (int)j0;
                    int j1i = (int)j1;

                    d[IX(xD, yD)] =
                      s0 * (t0 * d0[IX(i0i, j0i)] + t1 * d0[IX(i0i, j1i)]) +
                      s1 * (t0 * d0[IX(i1i, j0i)] + t1 * d0[IX(i1i, j1i)]);
                }
            }

            SetBnd(b, d);
        }

        private void SetBnd(int b, float[] x)
        {

            for (int xD = 1; xD < Size - 1; xD++)
            {
                x[IX(xD, 0)] = b == 2 ? -x[IX(xD, 1)] : x[IX(xD, 1)];
                x[IX(xD, Size - 1)] = b == 2 ? -x[IX(xD, Size - 2)] : x[IX(xD, Size - 2)];
            }
            for (int yD = 1; yD < Size - 1; yD++)
            {
                x[IX(0, yD)] = b == 1 ? -x[IX(1, yD)] : x[IX(1, yD)];
                x[IX(Size - 1, yD)] = b == 1 ? -x[IX(Size - 2, yD)] : x[IX(Size - 2, yD)];
            }

            x[IX(0, 0)] = 0.5f * (x[IX(1, 0)] + x[IX(0, 1)]);
            x[IX(0, Size - 1)] = 0.5f * (x[IX(1, Size - 1)] + x[IX(0, Size - 2)]);
            x[IX(Size - 1, 0)] = 0.5f * (x[IX(Size - 2, 0)] + x[IX(Size - 1, 1)]);
            x[IX(Size - 1, Size - 1)] = 0.5f * (x[IX(Size - 2, Size - 1)] + x[IX(Size - 1, Size - 2)]);
        }

        public void Step()
        {
            var diff = Diffusion;
            var dt = Dt;
            var Vx = this.Vx;
            var Vy = this.Vy;
            var Vx0 = this.Vx0;
            var Vy0 = this.Vy0;
            var s = S;
            var density = Density;

            Diffuse(1, Vx0, Vx, diff, dt);
            Diffuse(2, Vy0, Vy, diff, dt);

            Project(Vx0, Vy0, Vx, Vy);

            Advect(1, Vx, Vx0, Vx0, Vy0, dt);
            Advect(2, Vy, Vy0, Vx0, Vy0, dt);

            Project(Vx, Vy, Vx0, Vy0);
            Diffuse(0, s, density, diff, dt);
            Advect(0, density, s, Vx, Vy, dt);
        }

        public void Render(IRenderer renderPixels)
        {
            for (int xD = 0; xD < Size; xD++)
            {
                for (int yD = 0; yD < Size; yD++)
                {
                    //var x = xD;
                    //var y = yD;

                    var d = Density[IX(xD, yD)] > 254 ? 255 : Density[IX(xD, yD)];
                    renderPixels.DrawPixel(xD, yD, new Color(0, 0, 0, (byte)d));

                    //hslColor.SetRGB(255, 0, 0);
                    //hslColor.Hue = d % 255;
                    //renderPixels.DrawPixel(xD, yD, hslColor);
                }
            }
        }
    }
}
