using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public float Suly { get; private set; }
        public SulyozottEgeszGrafEl(int Honnan, int Hova, float suly) : base(Honnan, Hova)
        {
            this.Suly = suly;
        }
    }
    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n;
        float[,] M;

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    M[i, j] = float.NaN;
        }

        public int CsucsokSzama
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
                        if (!float.IsNaN(M[i, j]))
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

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (!float.IsNaN(M[i, j]))
                        {
                            SulyozottEgeszGrafEl el = new SulyozottEgeszGrafEl(i, j, M[i, j]);
                            elek.Beszur(el);
                        }
                    }
                }
                return elek;
            }
        }


        public float Suly(int honnan, int hova)
        {
            if (float.IsNaN(M[honnan, hova]))
            {
                throw new NincsElKivetel();
            }


            return M[honnan, hova];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();

            for (int j = 0; j < n; j++)
            {
                if (!float.IsNaN(M[csucs, j]))
                {
                    szomszedok.Beszur(j);
                }
            }

            return szomszedok;
        }

        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return !float.IsNaN(M[honnan, hova]);
        }
    }

    public class Utkereses
    {
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start)
        {
            Szotar<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (ez, ennel) => L.Kiolvas(ez) < L.Kiolvas(ennel));

            g.Csucsok.Bejar(x =>
            {
                L.Beir(x, float.MaxValue);
                S.Sorba(x);
            });
            L.Beir(start, 0);
            S.Frissit(start);

            while (!S.Ures)
            {
                V u = S.Sorbol();


                g.Szomszedai(u).Bejar(x =>
                {
                    float ujTav = L.Kiolvas(u) + g.Suly(u, x);

                    if (ujTav < L.Kiolvas(x))
                    {
                        L.Beir(x, ujTav);
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }

            return L;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where E : IComparable where V : IComparable
        {
            Szotar<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            Szotar<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            FaHalmaz<V> H = new FaHalmaz<V>();
            
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(
                g.CsucsokSzama,
                (ez, ennel) => K.Kiolvas(ez) < K.Kiolvas(ennel));

            g.Csucsok.Bejar(x =>
            {
                K.Beir(x, float.MaxValue);
                S.Sorba(x);
                H.Beszur(x);
            });

            K.Beir(start, 0);
            S.Frissit(start);

            while (!S.Ures)
            {
                V u = S.Sorbol();
                H.Torol(u);

                Halmaz<V> szomszedok = g.Szomszedai(u);

                szomszedok.Bejar(szomszed =>
                {
                    if (H.Eleme(szomszed) && g.Suly(u, szomszed) < K.Kiolvas(szomszed))
                    {
                        K.Beir(szomszed, g.Suly(u, szomszed));
                        P.Beir(szomszed, u);
                        S.Frissit(szomszed);
                    }
                });
            }
            return P;
        }
        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g) where E : SulyozottGrafEl<V>, IComparable
        {
            Halmaz<E> A = new FaHalmaz<E>();
            Szotar<V, int> vhalmaz = new HasitoSzotarTulcsordulasiTerulettel<V, int>(g.CsucsokSzama);
            int i = 0;
            g.Csucsok.Bejar(x => { 
                vhalmaz.Beir(x, i++); 
            });

            KupacPrioritasosSor<E> Sor = new KupacPrioritasosSor<E>(
            g.ElekSzama,
            (e1, e2) =>
            {
                if (e1.Suly < e2.Suly) 
                    return true;
                return false;
            }
            );

            g.Elek.Bejar(el => Sor.Sorba(el));

            void UnionSets(V u, V v)
            {
                int setU = vhalmaz.Kiolvas(u);
                int setV = vhalmaz.Kiolvas(v);

                g.Csucsok.Bejar(x =>
                {
                    if (vhalmaz.Kiolvas(x) == setV)
                    {
                        vhalmaz.Beir(x, setU);
                    }
                });
            }

            while (!Sor.Ures)
            {
                var el = Sor.Sorbol();
                V u = el.Honnan;
                V v = el.Hova;

                if (vhalmaz.Kiolvas(u) != vhalmaz.Kiolvas(v))
                {
                    A.Beszur(el);
                    UnionSets(u, v);
                }
                int j = 0;
                A.Bejar(x => j++);
                if (j == g.CsucsokSzama - 1) 
                    break;
            }

            return A;
        }
    }
}

