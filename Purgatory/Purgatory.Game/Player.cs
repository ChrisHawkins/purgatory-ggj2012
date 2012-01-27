
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Physics;

    public class Player : IMoveable
    {
        private float speed;
        private KeyboardManager controls;
        private Vector2 direction;
        private Sprite sprite;

        private Rectangle collisionRectangle;

        public Player(Sprite sprite, KeyboardManager controlScheme)
        {
            this.sprite = sprite;
            this.controls = controlScheme;
            this.collisionRectangle = new Rectangle(0, 0, Level.TileWidth, Level.TileWidth);
        }

        public Vector2 Position { get; set; }

        public void Update(GameTime time)
        {
            this.sprite.UpdateAnimation(time);

            this.UpdateMovement();
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.sprite.Draw(batch, bounds.AdjustPoint(this.Position));
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
