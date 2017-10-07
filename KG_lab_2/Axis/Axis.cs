using System;
using System.Drawing;

namespace KG_lab_2.Axis
{
    public abstract class Axis
    {
        protected WorldScreenConverter Converter;
        protected Graphics G;
        protected Pen GridPen = new Pen(Color.DarkGray, 1);
        protected Pen MainPen = new Pen(Color.Black, 2);

        protected Axis(WorldScreenConverter converter, Graphics g)
        {
            Converter = converter;
            G = g;
        }
        
        public static void Step(ref double k, ref double h, out int m, out int n)
        {
            var hWu = h / k;

            n = (int)Math.Floor(Math.Log10(hWu));

            var mFl = hWu / Math.Pow(10.0, n);

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

            var worldStep = m * Math.Pow(10.0, n); // WU

            h = worldStep * k ;
            k = worldStep;
        }


        public abstract void DrawMainLine();
    }
    
}