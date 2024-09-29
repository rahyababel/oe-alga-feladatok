using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> : Verem<T>
    {

        T[] E;
        int n = 0;
        public bool Ures
        {
            get
            {
                return n == 0;
            }
        }
        public TombVerem(int meret)
        {
            E = new T[meret];

        }
        public void Verembe(T ertek)
        {
            if (n < E.Length)
            {
                E[n] = ertek;
                n++;

            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Verembol()
        {
            if (n > 0)
            {
                T ertek = E[n - 1];
                n--;
                return ertek;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }

        public T Felso()
        {
            if (!Ures)
                return E[n - 1];
            else
                throw new NincsElemKivetel();
        }
    }
    public class TombSor<T> : Sor<T>
    {
        T[] E;
        int n = 0;
        int u = -1;
        int e = 0;

        public TombSor(int meret)
        {
            E = new T[meret];

        }
        public bool Ures
        {
            get
            {
                return n == 0;
            }
        }

        public T Elso()
        {
            if (!Ures)
                return E[e];
            else
                throw new NincsElemKivetel();
        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                u = (u + 1) % E.Length;
                E[u] = ertek;
                n++;
            }
            else
            {
                throw new NincsHelyKivetel();
            }
        }

        public T Sorbol()
        {
            if (!Ures)
            {
                T ertek = E[e];
                e = (e + 1) % E.Length;
                n--;
                return ertek;
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }
    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        T[] E;
        int n = 0;
        public int Elemszam
        {
            get
            {
                return n;
            }
        }
        public TombLista() : this(10)
        {

        }


        public TombLista(int meret)
        {
            E = new T[meret];
        }


        private void TombBovites()
        {
            T[] ujTomb = new T[E.Length * 2];
            for (int i = 0; i < n; i++)
            {
                ujTomb[i] = E[i];
            }
            E = ujTomb;
        }

        public T Kiolvas(int index)
        {
            if (index >= n || index < 0)
                throw new HibasIndexKivetel();
            else
                return E[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index >= n || index < 0)
                throw new HibasIndexKivetel();
            E[index] = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            if (n == E.Length)
            {
                TombBovites();
            }
            E[n] = ertek;
            n++;
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > n)
                throw new HibasIndexKivetel();

            if (n == E.Length)
            {
                TombBovites();
            }

            for (int i = n; i > index; i--)
            {
                E[i] = E[i - 1];
            }

            E[index] = ertek;
            n++;
        }

        public void Torol(T ertek)
        {
            int i = 0;
            while (i < n)
            {
                if (E[i].Equals(ertek))
                {
                    for (int j = i; j < n; j++)
                    {
                        E[j] = E[j + 1];
                    }
                    n--;
                }
                else
                {
                    i++;
                }

            }
        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            TombListaBejaro<T> tombListaBejaro = new TombListaBejaro<T>(E, n);
            return tombListaBejaro;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class TombListaBejaro<T> : IEnumerator<T>
    {
        private T[] E;
        private int n;
        protected int aktualisIndex = -1;

        public TombListaBejaro(T[] E, int n)
        {
            this.E = E;
            this.n = n;
        }
        public T Current 
        { 
            get 
            {
                return E[aktualisIndex];
            } 
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {

        }

        public bool MoveNext()
        {

            if (aktualisIndex < n - 1)
            {
                aktualisIndex++;
                return true;
            }
            return false;
        }
        public void Reset()
        {
            aktualisIndex = -1;
        }
    }
}