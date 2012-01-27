
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Physics;
    using System.Collections.Generic;

    public class Player : IMoveable
    {
        private float speed;
        private int health;
        private KeyboardManager controls;
        private Vector2 direction;
        private Vector2 movementDirection;
        private Sprite sprite;

        private Sprite bulletSprite;
        private List<Bullet> bulletList;

        private float shootCooldown;
        private float shootTimer;

        private Rectangle collisionRectangle;


        public Player()
        {
            this.Level = new Level("LifeMaze00");
            this.collisionRectangle = new Rectangle(0, 0, Level.TileWidth * 2, Level.TileWidth * 2);
            this.speed = 200;
            this.bulletList = new List<Bullet>();
            this.direction = new Vector2(0, 1);
            this.shootCooldown = 0.2f;
        }

        public void Initialize(KeyboardManager controlScheme, Sprite sprite, Sprite bulletSprite)
        {
            this.controls = controlScheme;
            this.sprite = sprite;
            this.bulletSprite = bulletSprite;
        }

        public Vector2 Position { get; set; }

        public Vector2 LastPosition { get; set; }

        public void Update(GameTime time)
        {
            this.controls.Update();
            this.UpdateMovement(time);

            // Update player direction. Dont change if movement direction has no length
            if (movementDirection.LengthSquared() != 0)
            {
                this.direction = movementDirection;
                this.sprite.PlayAnimation = true;
            }
            else
            {
                this.sprite.PlayAnimation = false;
            }

            this.UpdateShoot(time);
            this.sprite.UpdateAnimation(time);
        }

        private void UpdateShoot(GameTime time)
        {
            this.shootTimer += (float)time.ElapsedGameTime.TotalSeconds;
            if (this.controls.ShootControlPressed() && this.shootTimer > this.shootCooldown)
            {
                Vector2 bulletPos = this.Position + new Vector2(this.sprite.Width / 2.0f, this.sprite.Height / 2.0f) * direction;
                Bullet b = new Bullet(bulletPos, this.direction, this.speed * 7f, bulletSprite);
                this.bulletList.Add(b);
                this.shootTimer = 0.0f;
            }

            foreach (var bullet in bulletList)
            {
                bullet.Update(time);
            }
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.sprite.Draw(batch, bounds.AdjustPoint(this.Position));
            foreach(var bullet in bulletList)
            {
                bullet.Draw(batch, bounds);
            }
        }

        private void UpdateMovement(GameTime time)
        {
            movementDirection = new Vector2();
            if (controls.UpControlPressed())
            {
                movementDirection.Y -= 1;
            }

            if (controls.DownControlPressed())
            {
                movementDirection.Y += 1;
            }

            if (controls.LeftControlPressed())
            {
                movementDirection.X -= 1;
            }

            if (controls.RightControlPressed())
            {
                movementDirection.X += 1;
            }

            if (movementDirection.LengthSquared() != 0)
            {
                movementDirection.Normalize();
            }

            this.LastPosition = this.Position;
            this.Position += movementDirection * speed * (float)time.ElapsedGameTime.TotalSeconds;

            this.CheckForCollisions();
        }

        private void CheckForCollisions()
        {
            //List<Rectangle> possibleRectangles = Level.GetPossibleRectangles(
        }

        public Level Level { get; private set; }

        public Rectangle CollisionRectangle
        {
            get { return this.collisionRectangle; }
        }
    }
}
