using System.Drawing;

namespace KG_lab_2
{
    public class WorldScreenConverter
    {
        private float X1 { get; } 
        private float X2 { get; }
        private float Y1 { get; }
        private float Y2 { get; } // мировая система координат
        private RectangleF Screen { get; } // экранная система координат

        public WorldScreenConverter(float x1, float x2, float y1, float y2, RectangleF screen)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            Screen = screen;
        }

        public float WorldToScreenX(float x)
        {
            return Screen.Left + (int) ((x - X1) / (X2 - X1)) * Screen.Width;
        }
        
        public float WorldToScreenY(float y)
        {
            return Screen.Bottom + 1 - (int) ((y - Y1) / (Y2 - Y1)) * Screen.Height;
        }

        public PointF WorldToScreen(float x, float y)
        {
            return new PointF(WorldToScreenX(x), WorldToScreenY(y));
        }

        public float ScreenToWorldX(float x)
        {
            return X1 + (int) ((x - Screen.Left) / Screen.Width) * (X2 - X1);
        }
       
        public float ScreenToWorldY(float y)
        {
            return Y2 - (int) ((y - Screen.Top) / Screen.Height) * (Y2 - Y1);
        }
        
        public PointF ScreenToWorld(float x, float y)
        {
            return new PointF(ScreenToWorldX(x), ScreenToWorldY(y));
        }
    }
}