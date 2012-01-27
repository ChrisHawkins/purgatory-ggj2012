
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Physics;

    public class Player : IMoveable
    {
        private Rectangle collisionRectangle;

        public Player()
        {
            this.collisionRectangle = new Rectangle(0, 0, Level.TileWidth, Level.TileWidth);
        }

        public Point Position { get; set; }

        public void Update()
        {

        }

        public Level Level { get; private set; }

        public Rectangle CollisionRectangle
        {
            get { return this.collisionRectangle; }
        }
    }
}
