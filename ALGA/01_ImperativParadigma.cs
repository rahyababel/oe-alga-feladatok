using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        void Vegrehajtas();
    }

    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel(string message) : base(message) { }
    }
    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n;

        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n < tarolo.Length) {

                tarolo[n] = elem;
                n++;
            }
            else
            {
                throw new TaroloMegteltKivetel("Megtelt a tarolo!");
            }
        }
        public virtual void  MindentVegrehajt()
        {
            for (int i = 0; i < n; i++) 
            {
                tarolo[i].Vegrehajtas();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            FeladatTaroloBejaro<T> bejaro = new FeladatTaroloBejaro<T>(tarolo, n);
            return bejaro;
        }   

    }

    public interface IFuggo
    {
        bool FuggosegTeljesul { get; }
    }
    
    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo 
    {
        public FuggoFeladatTarolo(int meret) : base(meret)
        {
            tarolo = new T[meret];
        }
        override public void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul) 
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T> where T : IVegrehajthato
    {
        private T[] tarolo;
        private int n;
        private int aktualisIndex = -1;
        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public T Current
        {
            get
            {
                return tarolo[aktualisIndex];
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext()
        {
            if (aktualisIndex < n)
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

        public void Dispose() 
        {

        }
    }
}
