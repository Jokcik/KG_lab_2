using System;
using System.Drawing;

namespace KG_lab_2.Axis
{
    public abstract class Axis
    {
        protected WorldScreenConverter Converter;
        protected Graphics G;
        protected Pen GridPen = new Pen(Color.Black, 1);
        protected Pen MainPen = new Pen(Color.Black, 2);

        protected Axis(WorldScreenConverter converter, Graphics g)
        {
            Converter = converter;
            G = g;
        }
        
        public static void Step(ref double k, ref double h, out int m, out int n)
        {
            // k -- px/WU
            // h -- px, preferred screen step
            // m -- mantissa
            // n -- order

            // |m * (10^n) WU * k px/WU - h| --> min
            // m @ {1, 2, 5}, n @ Z

            // (h/k) -- WU, preferred world step
            double hWu = h / k;

            // n = [lg(h_wu)]
            n = (int)Math.Floor(Math.Log10(hWu));

            // m = h / 10^n
            double mFl = hWu / Math.Pow(10.0, n);

            if (mFl < 2)
                m = mFl - 1 < 2 - mFl ? 1 : 2;
            else if (mFl < 5)
                m = mFl - 2 < 5 - mFl ? 2 : 5;
            else
                m = mFl - 5 < 10 - mFl ? 5 : 10;

            if (m == 10)
            {
                m = 1;
                n++;
            }

            double worldStep = m * Math.Pow(10.0, n); // WU

            h = worldStep /* WU */ * k /* px/WU */;
            k = worldStep; // meant px/WU
        }


        public abstract void DrawMainLine();
    }
    
}