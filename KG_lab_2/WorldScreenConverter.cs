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

        public float WorldToScreenX(float x)
        {
            return Screen.Left + (int) ((x - World.Left) / World.Width * Screen.Width);
        }
        
        public float WorldToScreenY(float y)
        {
            return Screen.Top - (int) ((y - World.Bottom) / World.Height * Screen.Height);
        }

        public PointF WorldToScreen(float x, float y)
        {
            return new PointF(WorldToScreenX(x), WorldToScreenY(y));
        }

        public float ScreenToWorldX(float x)
        {
            return World.Left + (int) ((x - Screen.Left) / Screen.Width) * World.Width;
        }
       
        public float ScreenToWorldY(float y)
        {
            return World.Bottom + 1 - (int) ((y - Screen.Top) / Screen.Height) * World.Height;
        }
        
        public PointF ScreenToWorld(float x, float y)
        {
            return new PointF(ScreenToWorldX(x), ScreenToWorldY(y));
        }
    }
}