using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

        private Point _point = new Point(0, 0);
        private List<PointF> _list = new List<PointF>();
        private RectangleF lastRectangleF = new RectangleF();
        
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

        // ReSharper disable CompareOfFloatsByEqualityOperator
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var mainRectangle = g.VisibleClipBounds;

            if (mainRectangle.Width != lastRectangleF.Width || mainRectangle.Height != lastRectangleF.Height)
            {
                _list.Clear();
            }
            lastRectangleF = mainRectangle;
            InitY(mainRectangle);
            
            var converter = new WorldScreenConverter(
                new Rectangle(40, 40, (int)mainRectangle.Width - 80, (int)mainRectangle.Height - 80),
                new RectangleF(_xMin, _yMin, _xMax - _xMin, _yMax - _yMin)
            );

            var xAxis = new XAxis(converter, g, _sizeGrid);
            var yAxis = new YAxis(converter, g, _sizeGrid);
            xAxis.DrawMainLine();
            yAxis.DrawMainLine();
            
            if (_list.Count > 0)
            {
                g.DrawLines(new Pen(Color.Red, 2), _list.ToArray());
            }
            else
            {
                var p1 = converter.WorldToScreen(converter.World.Left, (float)_func(converter.World.Left));
                var dx = converter.World.Width / converter.Screen.Width;
                _list.Add(p1);
                for (var x = converter.World.Left + dx; x < converter.World.Right; x += dx)
                {
                    p1 = converter.WorldToScreen(x, (float) _func(x));
                    _list.Add(p1);
                }
                g.DrawLines(new Pen(Color.Red, 2), _list.ToArray());
            }

            if (_point.X != 0 && _point.Y >= 20)
            {
                var screenText = $@"Экранные координаты: ({_point.X}, {_point.Y})";
                var worldText =
                    $@"Мировые координаты: ({converter.ScreenToWorldX(_point.X).ToString(CultureInfo.InvariantCulture)}, {
                            converter.ScreenToWorldY(_point.Y).ToString(CultureInfo.InvariantCulture)
                        })";
                g.DrawString($"{screenText}\n{worldText}",
                    new Font(FontFamily.GenericMonospace, 9),
                Brushes.Black, new PointF(_point.X, _point.Y - 30));
            }
            
            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _point = e.Location;
            Invalidate();
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _point = new Point(0, 0);
            Invalidate();
            base.OnMouseLeave(e);
        }
    }
}