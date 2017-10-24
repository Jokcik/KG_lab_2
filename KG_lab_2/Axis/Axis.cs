using System;
using System.Drawing;
using System.Linq;

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
            var n1 = Math.Log10(hWu);
            var n2 = Math.Log10(hWu / 2);
            var n5 = Math.Log10(hWu / 5);

            var n1P = Math.Ceiling(n1);
            var n1M = Math.Floor(n1);
            
            var n2P = Math.Ceiling(n2);
            var n2M = Math.Floor(n2);
            
            var n5P = Math.Ceiling(n5);
            var n5M = Math.Floor(n5);
            
            
            var mass = new[]
            {
                hWu - Math.Pow(10, n1M), 
                Math.Pow(10, n1P)- hWu, 
                
                hWu - 2 * Math.Pow(10, n2M), 
                2 * Math.Pow(10, n2P) - hWu,
                
                hWu - 5 * Math.Pow(10, n5M), 
                5 * Math.Pow(10, n5P) - hWu,
            };
            var res = mass.Min();
            m = 1;
            if (Math.Abs(res - mass[0]) < 0.0000000001) k = Math.Pow(10, n1M);
            else if (Math.Abs(res - mass[1]) < 0.0000000001) k = Math.Pow(10, n1P);
            else if (Math.Abs(res - mass[2]) < 0.0000000001) k = (2 * Math.Pow(10, n2M));
            else if (Math.Abs(res - mass[3]) < 0.0000000001) k = (2 * Math.Pow(10, n2P));
            else if (Math.Abs(res - mass[4]) < 0.0000000001) k = (5 * Math.Pow(10, n5M));
            else if (Math.Abs(res - mass[5]) < 0.0000000001) {k =  (5 * Math.Pow(10, n5P));}
        }


        public abstract void DrawMainLine();
    }
    
}