using System.Drawing;

namespace KG_lab_2.Axis
{
    public class YAxis : Axis
    {
        public YAxis(WorldScreenConverter converter, Graphics g) : base(converter, g)
        {
        }        

        public override void DrawMainLine(double step)
        {
            var x = Converter.Screen.Width / 2;
            const int yl = 0;
            var yr = Converter.Screen.Height;
            G.DrawLine(Pen, x, yl, x, yr);   
            
            
        }
    }
}