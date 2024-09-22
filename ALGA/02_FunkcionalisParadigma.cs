using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel { get; set; }
        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {

        }
        public IEnumerator<T> GetEnumerator()
        {
            // FeltetelesFeladatTaroloBejaro visszaadása
            FeltetelesFeladatTaroloBejaro<T> bejaro = new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel = BejaroFeltetel ?? (x => true));
            return bejaro;
        }


        public void FeltetelesVegrehajtas(Func<T, bool> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }

        }

    }
    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        private T[] tarolo;
        private int n;
        protected int aktualisIndex = -1;
        private Func<T, bool> bejaroFeltetel;
        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> feltetel)
        {
            this.tarolo = tarolo;
            this.n = n;
            bejaroFeltetel = feltetel;
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
            while (aktualisIndex < n - 1)
            {
                aktualisIndex++;
                if (bejaroFeltetel(tarolo[aktualisIndex]))
                {
                    return true;
                }

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