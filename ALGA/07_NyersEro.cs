using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }

        public int OsszSuly(bool[] pakolas)
        {
            int osszSuly = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    osszSuly += w[i];
                }
            }
            return osszSuly;
        }

        public double OsszErtek(bool[] pakolas)
        {
            double osszErtek = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i])
                {
                    osszErtek += p[i];
                }
            }
            return osszErtek;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        private int m;
        private Func<int, T> generator;
        private Func<T, double> josag;
        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, double> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
            this.LepesSzam = 0;
        }

        public T OptimalisMegoldas()
        {
            T optimalisMegoldas = default(T);
            double maxErtek = double.MinValue;

            for (int i = 1; i <= m; i++)
            {
                T megoldas = generator(i);
                double ertek = josag(megoldas);
                LepesSzam++;
                if (ertek > maxErtek)
                {
                    maxErtek = ertek;
                    optimalisMegoldas = megoldas;
                }
            }

            return optimalisMegoldas;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        private HatizsakProblema problema;
        public int LepesSzam { get; private set; }

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] Generator(int i)
        {
            int szam = i - 1;
            bool[] K = new bool[problema.n];
            for (int j = 0; j < problema.n; j++)
            {
                K[j] = ((Math.Floor(szam / Math.Pow(2, j)) % 2) == 1);
            }
            return K;
        }

        public double Josag(bool[] pakolas)
        {
            if (!problema.Ervenyes(pakolas))
            {
                return -1;
            }
            return problema.OsszErtek(pakolas);
        }

        public bool[] OptimalisMegoldas()
        {
            int lehetsegesMegoldasok = (int)Math.Pow(2, problema.n);
            NyersEro<bool[]> megoldo = new NyersEro<bool[]>(
                lehetsegesMegoldasok,
                Generator,
                Josag
            );

            bool[] optimalisPakolas = megoldo.OptimalisMegoldas();
            LepesSzam = megoldo.LepesSzam;
            return optimalisPakolas;
        }

        public double OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }
}