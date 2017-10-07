using System;
using System.Drawing;
using System.Windows.Forms;

namespace KG_lab_2
{
    public sealed class Graphic : Form
    {
        private int _xMax;
        private int _xMin;
        private int _step;
        private Func<double, double> _func;


        public Graphic(int step, int xMax, int xMin, Func<double, double> func)
        {
            _step = step;
            _xMax = xMax;
            _xMin = xMin;
            _func = func;

            Text = @"График";
            Location = new Point(400, 200);
            StartPosition = FormStartPosition.Manual;
        }
           
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            
            base.OnPaint(e);
        }
    }
}