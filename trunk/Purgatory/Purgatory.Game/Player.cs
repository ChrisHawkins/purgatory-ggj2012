
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
using Purgatory.Game.Controls;
    using Purgatory.Game.Physics;

    public class Player : IMoveable
    {
        private float speed;
        private KeyboardManager controls;
        private Vector2 direction;

        private Rectangle collisionRectangle;

        public Player(KeyboardManager controlScheme)
        {
            this.controls = controlScheme;
            this.collisionRectangle = new Rectangle(0, 0, Level.TileWidth, Level.TileWidth);
        }

        public Vector2 Position { get; set; }
        

        public void Update()
        {
            this.UpdateMovement();
        }

        private void UpdateMovement()
        {
            direction = new Vector2();
            if (controls.UpControlPressed())
            {
                direction.Y += 1;
            }

            if (controls.DownControlPressed())
            {
                direction.Y -=1;
            }

            if (controls.LeftControlPressed())
            {
                direction.X -= 1;
            }

            if (controls.RightControlPressed())
            {
                direction.X += 1;
            }

            this.Position += direction * speed;
        }

        public Level Level { get; private set; }

        public Rectangle CollisionRectangle
        {
            get { return this.collisionRectangle; }
        }
    }
}
