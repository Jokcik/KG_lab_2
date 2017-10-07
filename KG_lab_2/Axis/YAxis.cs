using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace KG_lab_2.Axis
{
    public class YAxis : Axis
    {
        private readonly List<double> _gridLines;
        
        public YAxis(WorldScreenConverter converter, Graphics g, int interval) : base(converter, g)
        {
            double k = converter.Screen.Height / converter.World.Height;
            double h = interval;
            int m, n;

            Step(ref k, ref h, out m, out n);

            _gridLines = new List<double>();

            var ymin = (int)Math.Ceiling(converter.World.Top / k);
            var ymax = (int)Math.Floor(converter.World.Bottom / k);

            for (var y = ymin; y <= ymax; y++) 
                _gridLines.Add(y * k);
        }        

        public override void DrawMainLine()
        {
            var xl = Converter.Screen.Left;
            var xr = Converter.Screen.Left + Converter.Screen.Width;
            var y = Converter.Screen.Top + Converter.Screen.Height / 2;
            G.DrawLine(MainPen, xl, y, xr, y);

            foreach (var markAbs in _gridLines){
                var markAbsScreen =
                    Converter.WorldToScreenY((float)markAbs);

                G.DrawLine(GridPen,
                    new PointF(Converter.Screen.Left, markAbsScreen),
                    new PointF(Converter.Screen.Right, markAbsScreen)
                );

                G.DrawString(markAbs.ToString(CultureInfo.InvariantCulture),
                    new Font(FontFamily.GenericMonospace, 10),
                    Brushes.Black, 
                    new PointF((Converter.Screen.Left + Converter.Screen.Right) / 2f, markAbsScreen - 15));
            }
            
        }
    }
}