using System.Drawing;

namespace KG_lab_2
{
    public class WorldScreenConverter
    {
        public RectangleF World { get; } // мировая система координат
        public Rectangle Screen { get; } // экранная система координат

        public WorldScreenConverter(Rectangle screen, RectangleF world)
        {
            World = world;
            Screen = screen;
        }

        public int WorldToScreenX(float x)
        {
            if (float.IsNaN(x)) return int.MaxValue;
            return Screen.Left + (int) ((x - World.Left) / World.Width * Screen.Width);
        }
        
        public int WorldToScreenY(float y)
        {
            if (float.IsNaN(y)) return int.MaxValue;
            return Screen.Top - (int) ((y - World.Bottom) / World.Height * Screen.Height);
        }

        public PointF WorldToScreen(float x, float y)
        {
            return new PointF(WorldToScreenX(x), WorldToScreenY(y));
        }

        public float ScreenToWorldX(int x)
        {
            return World.Left + ((float)x - Screen.Left) / Screen.Width * World.Width;
        }
       
        public float ScreenToWorldY(int y)
        {
            return World.Top - ((float)y - Screen.Bottom) / Screen.Height * World.Height;
        }
        
        public PointF ScreenToWorld(int x, int y)
        {
            return new PointF(ScreenToWorldX(x), ScreenToWorldY(y));
        }
    }
}