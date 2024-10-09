using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> where T : IComparable
    {
        public T tart;
        public FaElem<T> bal;
        public FaElem<T> jobb;

        public FaElem(T tart, FaElem<T> bal, FaElem<T> jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable
    {
        FaElem<T>? gyoker;
        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }


        protected void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet) 
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }

        protected void ReszfaBejarasInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPreOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }

        protected void ReszfaBejarasPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }

        protected static FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
            {
                FaElem<T> uj = new FaElem<T>(ertek, null, null);
                return uj;
            }
            else
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else
                    if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabaBeszur(p.jobb, ertek);
                }
                return p;
            }
        }

        public void Beszur(T ertek)
        {
           gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        protected static bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                    return ReszfaEleme(p.bal, ertek);
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                        return ReszfaEleme(p.jobb, ertek);
                    else
                        return true;
                }

            }
            return false;
        }
        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        protected static FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                    p.bal = ReszfabolTorol(p.bal, ertek);
                else
                {
                    if (p.tart.CompareTo(ertek) < 0)
                        p.jobb = ReszfabolTorol(p.jobb, ertek);
                    else
                    {
                        if (p.bal == null)
                        {
                            p = p.jobb;
                        }
                        else
                            p.bal = KetGyerek(p, p.bal);
                    }
                }
                return p;
            }
            else
                throw new NincsElemKivetel();
        }

        static FaElem<T> KetGyerek(FaElem<T> e, FaElem<T> r)
        {
            FaElem<T> q;
            if (r.jobb != null)
            {
                r.jobb = KetGyerek(e, r.jobb);
                return r;
            }
            else
            {
                e.tart = r.tart;
                q = r.bal;
                return q;
            }
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }

    }
}

