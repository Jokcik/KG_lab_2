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
            var screenY = (int)Converter.WorldToScreenY(0);
            var screenX = (int)Converter.WorldToScreenX(0);
            var insertZeroX = !(screenX >= Converter.Screen.Right || screenX <= Converter.Screen.Left);
            var insertZeroY = !(screenY >= Converter.Screen.Bottom || screenY <= Converter.Screen.Top);
            
            foreach (var markAbs in _gridLines)
            {
                
                var x = Converter.Screen.Left;
                var markAbsScreen = Converter.WorldToScreenY((float)markAbs);

//                if (insertZeroX)
//                {
//                    x = Converter.WorldToScreenX(0);
//                }
                
                if (!Chart.PointIsValid(new PointF(0, markAbsScreen))) continue;
                
                G.DrawLine(GridPen,
                    new PointF(Converter.Screen.Left, markAbsScreen),
                    new PointF(Converter.Screen.Right, markAbsScreen)
                );
                
                var valueString = markAbs.ToString(CultureInfo.InvariantCulture);
                G.DrawString(markAbs.ToString(CultureInfo.InvariantCulture),
                    new Font(FontFamily.GenericMonospace, 10),
                    Brushes.Black, 
                    new PointF(x - (valueString.Length - 1) * 7 - 20, markAbsScreen - 10));
            }
            
            var xl = Converter.Screen.Left;
            var xr = Converter.Screen.Left + Converter.Screen.Width;
            var y = Converter.Screen.Bottom;

            if (insertZeroY)
            {
                y = screenY;
            }
        
            G.DrawLine(MainPen, xl, y, xr, y);
            G.DrawLine(MainPen, xr - 10, y - 4, xr, y);
            G.DrawLine(MainPen, xr - 10, y + 4, xr, y);
            G.DrawString("x",
                new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold),
                Brushes.Black,
                new PointF(xr + 10, y + 3));

        }
    }
}