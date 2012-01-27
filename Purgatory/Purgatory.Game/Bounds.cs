
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

    public struct Bounds
    {
        public bool EntireScreen;
        public Rectangle Rectangle;

        public Bounds(Rectangle rectangle)
        {
            this.EntireScreen = false;
            this.Rectangle = rectangle;
        }

        public static Bounds Screen = new Bounds() { EntireScreen = true, Rectangle = Rectangle.Empty };

        public Rectangle ToRectangle(GraphicsDevice device)
        {
            if (EntireScreen)
            {
                return device.Viewport.Bounds;
            }
            else
            {
                return this.Rectangle;
            }
        }
    }
}
