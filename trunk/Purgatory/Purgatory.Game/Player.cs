
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Physics;
    using System.Collections.Generic;
    using System;

    public class Player : IMoveable
    {
        private float speed;
       
        private KeyboardManager controls;
        private Vector2 direction;
        private Vector2 movementDirection;
        private Sprite sprite;
        private PlayerNumber playerNumber;
        private Vector2 bulletDirection;

        private List<float> xPenetrations;
        private List<float> yPenetrations;

        private Vector2 DashVelocity;

        private const float dashCooldownTime = 1;
        private float timeSinceLastDash = 0;

        private Sprite bulletSprite;

        public int Health { get; private set; }
        public int Energy { get; private set; }

        public List<Bullet> BulletList { get; private set; }

        private float shootCooldown;
        private float shootTimer;

        private Rectangle collisionRectangle;


        public Player(PlayerNumber playerNumber)
        {
            this.timeSinceLastDash = 100;
            this.speed = 350;
            this.playerNumber = playerNumber;
            this.Health = 20;
            this.Energy = 100;
            this.BulletList = new List<Bullet>();
            this.direction = new Vector2(0, 1);
            this.shootCooldown = 0.2f;

            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();
        }

        public void Initialize(KeyboardManager controlScheme, Sprite sprite, Sprite bulletSprite)
        {
            this.controls = controlScheme;
            this.sprite = sprite;
            this.bulletSprite = bulletSprite;
            this.collisionRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Vector2 LastPosition { get; set; }

        private Vector2 position;

        public void Update(GameTime gameTime)
        {
            timeSinceLastDash += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.controls.Update();
            this.UpdateMovement(gameTime);

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

            this.UpdateShoot(gameTime);
            this.sprite.UpdateAnimation(gameTime);

        }

        private void UpdateShoot(GameTime time)
        {
            this.shootTimer += (float)time.ElapsedGameTime.TotalSeconds;
            if (this.controls.ShootControlPressed() && this.shootTimer > this.shootCooldown)
            {
                Vector2 bulletPos = this.Position;
                Bullet b = new Bullet(bulletPos, this.bulletDirection, this.speed * 7f, bulletSprite, this.Level);
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
        }

        private void UpdateMovement(GameTime time)
        {
            if (this.DashVelocity == Vector2.Zero)
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

                    if (controls.DashControlPressed() && this.timeSinceLastDash > dashCooldownTime)
                    {
                        this.DashVelocity = speed * 5 * movementDirection;
                        this.timeSinceLastDash = 0;
                    }
                }
            }

            if (this.DashVelocity != Vector2.Zero)
            {
                this.LastPosition = this.Position;
                this.position += DashVelocity * (float)time.ElapsedGameTime.TotalSeconds;
                this.DashVelocity -= 10 * this.DashVelocity * (float)time.ElapsedGameTime.TotalSeconds;

                if (DashVelocity.LengthSquared() <= speed * speed)
                {
                    this.DashVelocity = Vector2.Zero;
                }
            }
            else
            {
                this.LastPosition = this.Position;
                this.position += movementDirection * speed * (float)time.ElapsedGameTime.TotalSeconds;
            }

            this.CheckForCollisions();
        }

        private void CheckForCollisions()
        {
            if (this.Position != this.LastPosition)
            {
                this.xPenetrations.Clear();
                this.yPenetrations.Clear();
                List<Rectangle> possibleRectangles = Level.GetPossibleRectangles(Position, LastPosition);

                foreach (Rectangle r in possibleRectangles)
                {
                    Vector2 penetration = CollisionSolver.SolveCollision(this, r);
                    if (penetration.X != 0)
                    {
                        this.xPenetrations.Add(penetration.X);
                    }
                    if (penetration.Y != 0)
                    {
                        this.yPenetrations.Add(penetration.Y);
                    }
                }

                if (xPenetrations.Count != 0 || yPenetrations.Count != 0)
                {
                    if (xPenetrations.Count >= yPenetrations.Count)
                    {
                        this.position.X -= xPenetrations[0];
                    }
                    if (yPenetrations.Count >= xPenetrations.Count)
                    {
                        this.position.Y -= yPenetrations[0];
                    }
                }
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

        internal void SetBulletDirection(Vector2 targetPosition)
        {
            this.bulletDirection = targetPosition - this.Position;
            this.bulletDirection.Normalize();
        }
    }
}
