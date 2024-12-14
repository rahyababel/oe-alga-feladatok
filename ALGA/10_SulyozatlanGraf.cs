using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl : GrafEl<int>, IComparable
    {
        public int Honnan { get; }
        public int Hova { get; }

        public EgeszGrafEl(int Honnan, int Hova)
        {
            this.Honnan = Honnan;
            this.Hova = Hova;
        }

        public virtual int CompareTo(object? obj)
        {
            if (obj != null && obj is EgeszGrafEl b)
            {
                if (Honnan != b.Honnan)
                    return Honnan.CompareTo(b.Honnan);
                else
                    return Hova.CompareTo(b.Hova);
            }
            else
                throw new InvalidOperationException();
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        int n = 0;
        bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n, n];
        }
        public int CsucsokSzam
        {
            get
            {
                return n;
            }
        }

        public int ElekSzama
        {
            get
            {
                int igazCount = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            igazCount++;
                        }
                    }
                }
                return igazCount;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }

        public Halmaz<EgeszGrafEl> Elek 
        {
            get
            {
                FaHalmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            EgeszGrafEl el = new EgeszGrafEl(i, j);
                            elek.Beszur(el);
                        }
                    }
                }
                return elek;
            }
        }

        public int CsucsokSzama
        {
            get
            {
                return n;
            }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
            {
                if (M[csucs, i])
                {
                    szomszedok.Beszur(i);
                }
            }
            return szomszedok;
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return M[honnan, hova];
        }
    }

    public static class GrafBejarasok
    { 
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            Sor<V> S = new LancoltSor<V>();
            Halmaz<V> F = new FaHalmaz<V>();

            S.Sorba(start);
            F.Beszur(start);

            while (!S.Ures)
            {
                V k = S.Sorbol();
                muvelet(k);

                g.Szomszedai(k).Bejar(x =>
                {
                    if (!F.Eleme(x))
                    {
                        S.Sorba(x);
                        F.Beszur(x);
                    }
                }
                );
            }

            return F;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            FaHalmaz<V> F = new FaHalmaz<V>();

            MelysegiBejarasRekurzio(g, start, F, muvelet);
            return F;
        }

        public static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet) where V : IComparable
        {
            Verem<V> S = new LancoltVerem<V>();
            
            F.Beszur(k);
            muvelet(k);

            g.Szomszedai(k).Bejar(x =>
            {
                if (!F.Eleme(x))
                {
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
                }
            }
            );
        }
    }
}

