using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        protected int n;
        protected int[] M;
        protected T[,] R;
        protected Func<int, T, bool> ft;
        protected Func<int, T, T[], bool> fk;
        protected Func<T[], float> josag;
        public int LepesSzam { get; private set; }

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        public T[] OptimalisMegoldas()
        {
            bool van = false;
            T[] E = new T[n];
            T[] O = new T[n];
            Backtrack(0, E, ref van, O);
            if (van)
                return O;
            else
                throw new Exception("Nincs megoldás");
        }

        void Backtrack(int szint, T[] E, ref bool van, T[] O)
        {
            int i = -1;
            while (i < M[szint] - 1)
            {
                i++;
                LepesSzam++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, n);
                                van = true;
                            }
                        }
                        else
                            Backtrack(szint + 1, E, ref van, O);
                    }
                }
            }
        }
    }
    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema problema;
        public int LepesSzam { get; private set; }
        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }
        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];
            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, R) => true;

            Func<int, bool, bool[], bool> fk = (szint, R, E) =>
            {
                int OsszSuly = 0;
                for (int i = 0; i < szint; i++)
                {
                    if (E[i])
                        OsszSuly += problema.w[i];
                }
                if (R)
                    OsszSuly += problema.w[szint];

                return OsszSuly <= problema.Wmax;
            };

            Func<bool[], float> josag = x =>
            {
                float OsszErtek = 0;
                for (int i = 0; i < problema.n; i++)
                {
                    if (x[i])
                        OsszErtek += problema.p[i];
                }
                return OsszErtek;
            };

            var opt = new VisszalepesesOptimalizacio<bool>(problema.n, M, R, ft, fk, josag);
            bool[] OptimalisMegoldas = opt.OptimalisMegoldas();
            LepesSzam = opt.LepesSzam;

            return OptimalisMegoldas;
        }

        public float OptimalisErtek()
        {
            float OsszErtek = 0;
            bool[] opt = OptimalisMegoldas();
            for (int i = 0; i < opt.Length; i++)
                if (opt[i])
                    OsszErtek += problema.p[i];
            return OsszErtek;
        }
    }
    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        private Func<int, T[], float> fb;
        public int LepesSzam { get; private set; } = 0;

        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag, Func<int, T[], float> fb) : base(n, M, R, ft, fk, josag)
        {
            this.fb = fb;
        }

        protected void Backtrack(int szint, T[] E, ref bool van, T[] O)
        {
            int i = -1;
            while (i < M[szint] - 1)
            {
                i++;
                LepesSzam++;

                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];

                        if (josag(E) + fb(szint, E) <= josag(O))
                            continue;

                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, n);
                                van = true;
                            }
                        }
                        else
                        {
                            Backtrack(szint + 1, E, ref van, O);
                        }
                    }
                }
            }
        }
    }
    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public int LepesSzam { get; private set; }
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema) 
        { 

        }

        public bool[] OptimalisMegoldas()
        {
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];
            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2;
                R[i, 0] = true;
                R[i, 1] = false;
            }

            Func<int, bool, bool> ft = (szint, R) => true;

            Func<int, bool, bool[], bool> fk = (szint, R, E) =>
            {
                int OsszSuly = 0;
                for (int i = 0; i < szint; i++)
                {
                    if (E[i])
                        OsszSuly += problema.w[i];
                }
                if (R)
                    OsszSuly += problema.w[szint];

                return OsszSuly <= problema.Wmax;
            };

            Func<bool[], float> josag = x =>
            {
                float OsszErtek = 0;
                for (int i = 0; i < problema.n; i++)
                {
                    if (x[i])
                        OsszErtek += problema.p[i];
                }
                return OsszErtek;
            };

            Func<int, bool[], float> fb = (szint, E) =>
            {
                float becsultErtek = 0;
                int OsszSuly = 0;

                for (int i = 0; i < szint; i++)
                {
                    if (E[i])
                        OsszSuly += problema.w[i];
                }

                for (int i = szint; i < problema.n; i++)
                {
                    if (OsszSuly + problema.w[i] <= problema.Wmax)
                    {
                        becsultErtek += problema.p[i];
                        OsszSuly += problema.w[i];
                    }
                    else
                    {
                        float maradekHelyAranya = (float)(problema.Wmax - OsszSuly) / problema.w[i];
                        becsultErtek += maradekHelyAranya * problema.p[i];
                        break;
                    }
                }
                return becsultErtek;
            };

            var opt = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(problema.n, M, R, ft, fk, josag, fb);
            bool[] OptimalisMegoldas = opt.OptimalisMegoldas();
            LepesSzam = opt.LepesSzam;

            return OptimalisMegoldas;
        }
    }
}
