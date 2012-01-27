
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
       
        private KeyboardManager controls;
        private Vector2 direction;
        private Vector2 movementDirection;
        private Sprite sprite;

        private HealthBar healthBar;

        private Sprite bulletSprite;

        public int Health { get; private set; }
        public List<Bullet> BulletList { get; private set; }

        private float shootCooldown;
        private float shootTimer;

        private Rectangle collisionRectangle;


        public Player()
        {
            this.speed = 200;
            this.Health = 100;
            this.BulletList = new List<Bullet>();
            this.direction = new Vector2(0, 1);
            this.shootCooldown = 0.2f;
            this.healthBar = new HealthBar(new Vector2(256, 768 - 40), this.Health);
        }

        public void Initialize(KeyboardManager controlScheme, Sprite sprite, Sprite bulletSprite)
        {
            this.controls = controlScheme;
            this.sprite = sprite;
            this.bulletSprite = bulletSprite;
            this.collisionRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
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

            this.healthBar.Update(this.Health);

        }

        private void UpdateShoot(GameTime time)
        {
            this.shootTimer += (float)time.ElapsedGameTime.TotalSeconds;
            if (this.controls.ShootControlPressed() && this.shootTimer > this.shootCooldown)
            {
                Vector2 bulletPos = this.Position + new Vector2(this.sprite.Width / 2.0f, this.sprite.Height / 2.0f) * direction;
                Bullet b = new Bullet(bulletPos, this.direction, this.speed * 7f, bulletSprite, this.Level);
                this.BulletList.Add(b);
                this.shootTimer = 0.0f;
            }

            List<Bullet> tmpBulletList = new List<Bullet>();
            foreach (var bullet in BulletList)
            {
                bullet.Update(time);
                if (!bullet.RemoveFromList)
                {
                    tmpBulletList.Add(bullet);
                }

            }

            BulletList = tmpBulletList;
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.sprite.Draw(batch, bounds.AdjustPoint(this.Position));

            foreach(var bullet in BulletList)
            {
                bullet.Draw(batch, bounds);
            }

            this.healthBar.Draw(batch, bounds);
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
            List<Rectangle> possibleRectangles = Level.GetPossibleRectangles(Position, LastPosition);

            foreach (Rectangle r in possibleRectangles)
            {
                CollisionSolver.SolveCollision(this, r);
            }
        }

        public Level Level { get; set; }

        public Rectangle CollisionRectangle
        {
            get { return GeometryUtility.GetAdjustedRectangle(this.Position, this.collisionRectangle); }
        }

        public void CheckBulletCollisions(List<Bullet> list)
        {
            foreach (var bullet in list)
            {
                if (this.CollisionRectangle.Intersects(bullet.CollisionRectangle))
                {
                    bullet.RemoveFromList = true;
                    this.Health -= 1;
                }
            }
        }
    }
}
