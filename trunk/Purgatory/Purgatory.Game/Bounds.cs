
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;

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
    }
}
