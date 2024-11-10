using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>
    {
        int n; 
        int[] M; 
        T[,] R; 
        Func<int, T, bool> ft; 
        Func<int, T, T[], bool> fk; 
        Func<T[], double> josag;

        public int LepesSzam { get; private set; } 

        public VisszalepesesOptimalizacio(int n, int[] M, T[,] R, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], double> josag)
        {
            this.n = n;
            this.M = M;
            this.R = R;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
            this.LepesSzam = 0;
        }

        private void Backtrack(int szint, T[] E, ref bool van, ref T[] O)
        {
            if (szint == n) 
            {
                LepesSzam++;
                if (!van || josag(E) > josag(O))
                {
                    van = true;
                    Array.Copy(E, O, n); 
                }
            }
            else
            {
                for (int i = 0; i < M[szint]; i++)
                {
                    if (ft(szint, R[szint, i]) && fk(szint, R[szint, i], E)) 
                    {
                        E[szint] = R[szint, i]; 
                        Backtrack(szint + 1, E, ref van, ref O);
                        E[szint] = default(T);
                    }
                }
            }
        }


        public T[] OptimalisMegoldas()
        {
            T[] E = new T[n];
            T[] O = new T[n];
            bool van = false;
            Backtrack(0, E, ref van, ref O); 
            return van ? O : null;
        }
    }

    public class VisszalepesesHatizsakPakolas
    {
        private HatizsakProblema problema;
        public int LepesSzam { get; private set; }

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
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