using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritas;

        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = E;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }

        public static int Bal(int i)
        {
            return i * 2 + 1;
        }

        public static int Jobb(int i)
        {
            return i * 2 + 2;
        }

        public static int Szulo(int i)
        {
            return (i - 1) / 2;
        }

        public void Kupacol(int i)
        {
            int bal = Bal(i);
            int jobb = Jobb(i);
            int max = i;

            if (bal < n && nagyobbPrioritas(E[bal], E[max]))
                max = bal;
            if (jobb < n && nagyobbPrioritas(E[jobb], E[max]))
                max = jobb;

            if (max != i)
            {
                T temp = E[i];
                E[i] = E[max];
                E[max] = temp;
                Kupacol(max);
            }
        }

        public void KupacotEpit()
        {
            for (int i = n / 2; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        public KupacRendezes(T[] E) : base(E, E.Length, (x, y) => x.CompareTo(y) > 0)
        { }

        public void Rendezes()
        {
            int tempN = n;
            for (int i = tempN - 1; i > 0; i--)
            {
                T temp = E[0];
                E[0] = E[i];
                E[i] = temp;
                n--;
                Kupacol(0);
            }
            n = tempN;
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public bool Ures => n == 0;

        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas)
            : base(new T[meret], 0, nagyobbPrioritas) { }

        private void KulcsotFelvisz(int i)
        {
            while (i > 0 && nagyobbPrioritas(E[i], E[Szulo(i)]))
            {
                var temp = E[i];
                E[i] = E[Szulo(i)];
                E[Szulo(i)] = temp;
                i = Szulo(i);
            }
        }

        public void Sorba(T ertek)
        {
            if (n == E.Length) throw new NincsHelyKivetel();
            E[n] = ertek;
            n++;
            KulcsotFelvisz(n - 1);
        }

        public T Sorbol()
        {
            if (Ures) throw new NincsElemKivetel();

            T gyoker = E[0];
            E[0] = E[n - 1];
            n--;
            Kupacol(0);
            return gyoker;
        }

        public T Elso()
        {
            if (Ures) throw new NincsElemKivetel();
            return E[0];
        }

        public void Frissit(T ertek)
        {
            int i = Array.IndexOf(E, ertek);
            if (i == -1) throw new NincsElemKivetel();

            E[i] = ertek;
            KulcsotFelvisz(i);
            Kupacol(i);
        }
    }
}
