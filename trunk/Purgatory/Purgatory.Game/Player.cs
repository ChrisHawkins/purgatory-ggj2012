
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


        public Player()
        {
            this.Level = new Level("LifeMaze00");
            this.collisionRectangle = new Rectangle(0, 0, Level.TileWidth, Level.TileWidth);
            this.speed = 50;
        }

        public void Initialize(KeyboardManager controlScheme, Sprite sprite)
        {
            this.controls = controlScheme;
            this.sprite = sprite;
        }

        public Vector2 Position { get; set; }


        public void Update(GameTime time)
        {
            this.controls.Update();
            this.UpdateMovement(time);
            this.sprite.UpdateAnimation(time);
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.Level.Draw(batch, bounds);
            this.sprite.Draw(batch, bounds.AdjustPoint(this.Position));
        }

        private void UpdateMovement(GameTime time)
        {
            direction = new Vector2();
            if (controls.UpControlPressed())
            {
                direction.Y -= 1;
            }

            if (controls.DownControlPressed())
            {
                direction.Y +=1;
            }

            if (controls.LeftControlPressed())
            {
                direction.X -= 1;
            }

            if (controls.RightControlPressed())
            {
                direction.X += 1;
            }

            this.Position += direction * speed * (float)time.ElapsedGameTime.TotalSeconds;
        }

        public Level Level { get; private set; }

        public Rectangle CollisionRectangle
        {
            get { return this.collisionRectangle; }
        }
    }
}
