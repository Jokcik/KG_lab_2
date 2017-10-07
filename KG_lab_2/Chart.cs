using System;
using System.Drawing;
using System.Windows.Forms;

namespace KG_lab_2
{
    public sealed class Chart : Form
    {
        private int _xMax;
        private int _xMin;
        private int _step;
        private float _yMax;
        private float _yMin;
        private readonly Func<double, double> _func;

        public Chart(int step, int xMax, int xMin, Func<double, double> func)
        {
            _step = step;
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

            // ReSharper disable once NotAccessedVariable
            var step = (max - min) / 100f;
            for (var i = min; i <= max; ++step)
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

            _yMax = (float) Math.Min(max, 600);
            _yMin = (float) Math.Min(min, 600);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var mainRectangle = g.VisibleClipBounds;
            var converter = new WorldScreenConverter(_xMin, _yMax, _xMax, _yMin,
                new RectangleF(0, 0, mainRectangle.Width, mainRectangle.Height)
            );
            
            
            base.OnPaint(e);
        }
    }
}