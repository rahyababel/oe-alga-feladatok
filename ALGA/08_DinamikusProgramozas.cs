using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        HatizsakProblema problema;
        public int LepesSzam { private set; get; }

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }
        private float[,] TablatFeltolt()
        {
            int n = problema.n;
            int Wmax = problema.Wmax;
            int[] w = problema.w;
            float[] p = problema.p;

            float[,] F = new float[n + 1, Wmax + 1];

            for (int t = 0; t <= n; t++)
            {
                for (int h = 0; h <= Wmax; h++)
                {
                    if (h == 0 || t == 0)
                    {
                        F[t, h] = 0;
                    }
                    else if (h >= w[t - 1])
                    {
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - w[t - 1]] + p[t - 1]);
                    }
                    else
                    {
                        F[t, h] = F[t - 1, h];
                    }
                    LepesSzam++;
                }
            }

            return F;
        }

        public float OptimalisErtek()
        {
            float[,] F = TablatFeltolt();
            return F[problema.n, problema.Wmax];
        }


        public bool[] OptimalisMegoldas()
        {
            int n = problema.n;
            int Wmax = problema.Wmax;
            int[] w = problema.w;

            float[,] F = TablatFeltolt();
            bool[] megoldas = new bool[n];
            int h = Wmax;

            for (int t = n; t > 0; t--)
            {
                if (h >= w[t - 1] && F[t, h] == F[t - 1, h - w[t - 1]] + problema.p[t - 1])
                {
                    megoldas[t - 1] = true;
                    h -= w[t - 1];
                }
                else
                {
                    megoldas[t - 1] = false;
                }
            }

            return megoldas;
        }
    }
}
