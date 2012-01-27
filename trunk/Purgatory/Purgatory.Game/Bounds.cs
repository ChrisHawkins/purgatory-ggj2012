
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public struct Bounds
    {
        public bool EntireScreen;
        public Rectangle Rectangle;
        public Vector2 Camera;

        public Bounds(Rectangle rectangle)
        {
            this.EntireScreen = false;
            this.Rectangle = rectangle;
            this.Camera = Vector2.Zero;
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

        public Vector2 AdjustPoint(Vector2 point)
        {
            if (this.EntireScreen)
            {
                return point;
            }
            else
            {
                // centre of bounds is 0,0
                point += Camera;

                point.X += Rectangle.X + Rectangle.Width / 2;
                point.Y += Rectangle.Y + Rectangle.Height / 2;
            }

            return point;
        }

        public Point AdjustPoint(Point point)
        {
            if (this.EntireScreen)
            {
                return point;
            }
            else
            {
                // centre of bounds is 0,0
                point.X += (int)Camera.X;
                point.Y += (int)Camera.Y;

                point.X += Rectangle.X + Rectangle.Width / 2;
                point.Y += Rectangle.Y + Rectangle.Height / 2;
            }

            return point;
        }

        public Rectangle AdjustRectangle(Rectangle rectangle)
        {
            rectangle.Location = AdjustPoint(rectangle.Location);
            return rectangle;
        }
    }
}
