using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using KG_lab_2.Axis;

namespace KG_lab_2
{
    public sealed class Chart : Form
    {
        private float _xMax;
        private float _xMin;
        private readonly int _sizeGrid;
        private float _yMax;
        private float _yMin;
        private readonly Func<double, double> _func;

        private Point _point = new Point(0, 0);
        private List<PointF> _list = new List<PointF>();
        private RectangleF _lastRectangleF;
        
        public Chart(int sizeGrid, float xMax, float xMin, Func<double, double> func, string textFunc)
        {
            _sizeGrid = sizeGrid;
            _xMax = xMax;
            _xMin = xMin;
            _func = func;

            Text = $@"График функции: {textFunc}";
            Location = new Point(400, 200);
            StartPosition = FormStartPosition.Manual;

            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            ResizeRedraw = true;
        }
        

        private static float NormalizeValue(double initialValue)
        {
            if (initialValue < float.MinValue || initialValue > float.MaxValue)
                return float.NaN;

            return (float)initialValue;
        }


        // ReSharper disable CompareOfFloatsByEqualityOperator
        private void InitY(RectangleF screen)
        {
            if (screen.Width != _lastRectangleF.Width || screen.Height != _lastRectangleF.Height)
            {
                _list.Clear();
            }
        
            var insert = _list.Count == 0;
            
            
            _yMin = float.MaxValue;
            _yMax = float.MinValue;

//            var n = screen.Width / (_xMax - _xMin);
            const int n = 1000;
            for (var i = 0; i < n; i++)
            {
                var x = _xMin + i * (_xMax - _xMin) / n;
                var value = NormalizeValue(_func(x));
                if (insert)
                {
                    _list.Add(new PointF(x, value));
                }       
                if (value < _yMin)
                    _yMin = value;
                if (value  > _yMax)
                    _yMax = value;
            }

            if (!(_yMax - _yMin <= Math.Abs(_yMin * 0.000001))) return;
            var middle = (_yMin + _yMax) / 2.0f;

            _yMax = middle + Math.Abs(middle) * 0.000001f;
            _yMin = middle - Math.Abs(middle) * 0.000001f;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var mainRectangle = g.VisibleClipBounds;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            InitY(mainRectangle);

            var converter = new WorldScreenConverter(
                new Rectangle(40, 40, (int)mainRectangle.Width - 80, (int)mainRectangle.Height - 80),
                new RectangleF(_xMin, _yMin, _xMax - _xMin, _yMax - _yMin)
            );

            if (mainRectangle.Width != _lastRectangleF.Width || mainRectangle.Height != _lastRectangleF.Height)
            {
                _list = _list.Select(f => converter.WorldToScreen(f.X, f.Y)).ToList();                
            }
            _lastRectangleF = mainRectangle;
    
            var xAxis = new XAxis(converter, g, _sizeGrid);
            var yAxis = new YAxis(converter, g, _sizeGrid);
            xAxis.DrawMainLine();
            yAxis.DrawMainLine();

            var lines = new List<PointF>();
            foreach (PointF p in _list)
            {
                if (!PointIsValid(p) && lines.Count > 0)
                {
                    g.DrawLines(new Pen(Color.Blue, 2), lines.ToArray());                                                       
                    lines.Clear();
                }
                else
                {
                    lines.Add(p);
                }
            }
            if (lines.Count > 0)
            {
                g.DrawLines(new Pen(Color.Blue, 2), lines.ToArray());
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
                
                g.DrawLine(Pens.Red, new Point(_point.X, converter.Screen.Top), new Point(_point.X, converter.Screen.Bottom));
                g.DrawLine(Pens.Red, new Point(converter.Screen.Left, _point.Y), new Point(converter.Screen.Right, _point.Y));
            }
            
            base.OnPaint(e);
        }

        public static bool PointIsValid(PointF p)
        {
            return !(p.Y >= int.MaxValue || p.Y <= int.MinValue);
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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            _xMin += e.Delta > 0 ? 0.1f : -0.1f;
            _xMax -= e.Delta > 0 ? 0.1f : -0.1f;
            _lastRectangleF = new RectangleF();
            Invalidate();
            base.OnMouseWheel(e);
        }
    }
}