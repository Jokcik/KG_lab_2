using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace KG_lab_2.Axis
{
    public class XAxis : Axis
    {
        private readonly List<double> _gridLines;
        
        public XAxis(WorldScreenConverter converter, Graphics g, int interval) : base(converter, g)
        {
            double k = converter.Screen.Width / converter.World.Width;
            double h = interval;
            int m, n;

            Step(ref k, ref h, out m, out n);

            _gridLines = new List<double>();

            int xmin = (int)Math.Ceiling(converter.World.Left / k);
            int xmax = (int)Math.Floor(converter.World.Right / k);

            for (int x = xmin; x <= xmax; x++)
                _gridLines.Add(x * k);

        }        

        public override void DrawMainLine(double step)
        {
            const int xl = 0;
            var xr = Converter.Screen.Width;
            var y = Converter.Screen.Height / 2;
            G.DrawLine(Pen, xl, y, xr, y);

            foreach (double markAbs in _gridLines){
                float markAbsScreen =
                    Converter.WorldToScreenX((float)markAbs);

                G.DrawLine(Pen,
                    new PointF(markAbsScreen, Converter.Screen.Top),
                    new PointF(markAbsScreen, Converter.Screen.Bottom)
                );
            }

        }

    }
}