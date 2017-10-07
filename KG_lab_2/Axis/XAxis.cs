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

            var xmin = (int) Math.Ceiling(converter.World.Left / k);
            var xmax = (int) Math.Floor(converter.World.Right / k);

            for (var x = xmin; x <= xmax; x++)
                _gridLines.Add(x * k);

        }

        public override void DrawMainLine()
        {
            foreach (var markAbs in _gridLines)
            {
                var markAbsScreen = Converter.WorldToScreenX((float) markAbs);
                var y = Converter.Screen.Bottom;
                if (Converter.World.Top <= 0 && Converter.World.Bottom >= 0)
                {
                    y = (int)Converter.WorldToScreenY(0);
                }

                G.DrawLine(GridPen,
                    new PointF(markAbsScreen, Converter.Screen.Top),
                    new PointF(markAbsScreen, Converter.Screen.Bottom)
                );

                var valueString = markAbs.ToString(CultureInfo.InvariantCulture);

                G.DrawString(markAbs.ToString(CultureInfo.InvariantCulture),
                    new Font(FontFamily.GenericMonospace, 10),
                    Brushes.Black,
                    new PointF(markAbsScreen - valueString.Length * 5, y));
            }

            var x = Converter.Screen.Left;
            var yl = Converter.Screen.Top;
            var yr = Converter.Screen.Top + Converter.Screen.Height;
            
            if (Converter.World.Top <= 0.0001 && Converter.World.Bottom >= 0.0001)
            {
                x = (int)Converter.WorldToScreenX(0);
            }
            
            G.DrawLine(MainPen, x, yl, x, yr);
            G.DrawLine(MainPen, x - 4, yl + 10, x, yl);
            G.DrawLine(MainPen, x + 4, yl + 10, x, yl);
            G.DrawString("y",
                new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold),
                Brushes.Black,
                new PointF(x + 3, yl - 25));

        }
    }
}