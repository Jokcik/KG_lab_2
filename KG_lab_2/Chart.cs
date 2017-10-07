using System;
using System.Drawing;
using System.Windows.Forms;
using KG_lab_2.Axis;

namespace KG_lab_2
{
    public sealed class Chart : Form
    {
        private readonly int _xMax;
        private readonly int _xMin;
        private readonly int _sizeGrid;
        private float _yMax;
        private float _yMin;
        private readonly Func<double, double> _func;

        public Chart(int sizeGrid, int xMax, int xMin, Func<double, double> func)
        {
            _sizeGrid = sizeGrid;
            _xMax = xMax;
            _xMin = xMin;
            _func = func;
            InitY();

            Text = @"График";
            Location = new Point(400, 200);
            StartPosition = FormStartPosition.Manual;

            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            ResizeRedraw = true;
        }

        private void InitY()
        {
            double max = int.MinValue;
            double min = int.MaxValue;

            var step = (_xMax - _xMin) / 100f;
            for (double i = _xMin; i <= _xMax; i += step)
            {
                var value = _func(i);
                if (value > max)
                {
                    max = value;
                }

                if (value < min)
                {
                    min = value;
                }
            }

            _yMax = (float) (max > int.MaxValue ? 600 : max); // TODO: переделать
            _yMin = (float) (min < int.MinValue ? 600 : min);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var mainRectangle = g.VisibleClipBounds;
            var converter = new WorldScreenConverter(
                new Rectangle(20, 20, (int)mainRectangle.Width - 40, (int)mainRectangle.Height - 40),
                new RectangleF(_xMin, _yMin, _xMax - _xMin, _yMax - _yMin)
            );

            var xAxis = new XAxis(converter, g, _sizeGrid);
            var yAxis = new YAxis(converter, g, _sizeGrid);
            xAxis.DrawMainLine();
            yAxis.DrawMainLine();
            
            PointF p1 = converter.WorldToScreen(converter.World.Left, (float)_func(converter.World.Left));
            var dx = converter.World.Width / converter.Screen.Width;

            for (var x = converter.World.Left + dx; x < converter.World.Right; x += dx)
            {
                var p2 = converter.WorldToScreen(x, (float)_func(x));
                g.DrawLine(new Pen(Color.Red, 2), p1, p2);
                p1 = p2;
            }
            
            base.OnPaint(e);
        }
    }
}