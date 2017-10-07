using System;
using System.Drawing;

namespace KG_lab_2.Axis
{
    public abstract class Axis
    {
        protected WorldScreenConverter Converter;
        protected Graphics G;
        protected Pen Pen = new Pen(Color.Black, 2);

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
            double h_wu = h / k;

            // n = [lg(h_wu)]
            n = (int)Math.Floor(Math.Log10(h_wu));

            // m = h / 10^n
            double m_fl = h_wu / Math.Pow(10.0, n);

            if (m_fl < 2)
                m = m_fl - 1 < 2 - m_fl ? 1 : 2;
            else if (m_fl < 5)
                m = m_fl - 2 < 5 - m_fl ? 2 : 5;
            else
                m = m_fl - 5 < 10 - m_fl ? 5 : 10;

            if (m == 10)
            {
                m = 1;
                n++;
            }

            double world_step = m * Math.Pow(10.0, n); // WU

            h = world_step /* WU */ * k /* px/WU */;
            k = world_step; // meant px/WU
        }


        public abstract void DrawMainLine(double step);
    }
    
}