using System;
using System.Drawing;
using System.Windows.Forms;
using KG_lab_2.Axis;

namespace KG_lab_2
{
    public sealed class Chart : Form
    {
        private readonly float _xMax;
        private readonly float _xMin;
        private readonly int _sizeGrid;
        private float _yMax;
        private float _yMin;
        private readonly Func<double, double> _func;

        private WorldScreenConverter _converter;
        
        public Chart(int sizeGrid, float xMax, float xMin, Func<double, double> func)
        {
            _sizeGrid = sizeGrid;
            _xMax = xMax;
            _xMin = xMin;
            _func = func;

            Text = @"График";
            Location = new Point(400, 200);
            StartPosition = FormStartPosition.Manual;

            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            ResizeRedraw = true;
        }

        private void InitY(RectangleF screen)
        {
            double max = int.MinValue;
            double min = int.MaxValue;

            var step = (_xMax - _xMin) / screen.Width;
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
            InitY(mainRectangle);
            
            _converter = new WorldScreenConverter(
                new Rectangle(40, 40, (int)mainRectangle.Width - 80, (int)mainRectangle.Height - 80),
                new RectangleF(_xMin, _yMin, _xMax - _xMin, _yMax - _yMin)
            );

            var xAxis = new XAxis(_converter, g, _sizeGrid);
            var yAxis = new YAxis(_converter, g, _sizeGrid);
            xAxis.DrawMainLine();
            yAxis.DrawMainLine();
            
            PointF p1 = _converter.WorldToScreen(_converter.World.Left, (float)_func(_converter.World.Left));
            var dx = _converter.World.Width / _converter.Screen.Width;

            for (var x = _converter.World.Left + dx; x < _converter.World.Right; x += dx)
            {
                var p2 = _converter.WorldToScreen(x, (float)_func(x));
                g.DrawLine(new Pen(Color.Red, 2), p1, p2);
                p1 = p2;
            }
            
            base.OnPaint(e);
        }
    }
}