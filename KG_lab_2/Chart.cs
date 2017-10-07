using System;
using System.Drawing;
using System.Windows.Forms;
using KG_lab_2.Axis;

namespace KG_lab_2
{
    public sealed class Chart : Form
    {
        private int _xMax;
        private int _xMin;
        private int _sizeGrid;
        private double _stepX;
        private double _stepY;
        private float _yMax;
        private float _yMin;
        private readonly Func<double, double> _func;

        public Chart(int sizeGrid, int xMax, int xMin, Func<double, double> func)
        {
            _sizeGrid = sizeGrid;
            _xMax = xMax;
            _xMin = xMin;
            _stepX = (xMax - xMin) / (double)sizeGrid;
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

            _yMax = (float) Math.Min(max, 600); // TODO: переделать
            _yMin = (float) Math.Min(min, 600);
            _stepY = (_yMax - _yMin) / _sizeGrid;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var mainRectangle = g.VisibleClipBounds;
            var converter = new WorldScreenConverter(
                new Rectangle(0, 0, (int)mainRectangle.Width, (int)mainRectangle.Height),
                new RectangleF(_xMin, _yMin, _xMax - _xMin, _yMax - _yMin)
            );

            var xAxis = new XAxis(converter, g, _sizeGrid);
            var yAxis = new YAxis(converter, g, _sizeGrid);
            xAxis.DrawMainLine(_stepX);
            yAxis.DrawMainLine(_stepY);
            
            base.OnPaint(e);
        }
    }
}