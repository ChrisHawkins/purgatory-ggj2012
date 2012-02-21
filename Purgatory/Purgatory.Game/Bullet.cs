
namespace Purgatory.Game
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Physics;
    using Purgatory.Game.Animation;

    public class Bullet : IMoveable
    {
        public Vector2 Direction;
        private int bounce;
        private float speed;
        private Sprite sprite;
        private Level level;
        private static List<float> xPenetrations = new List<float>();
        private static List<float> yPenetrations = new List<float>();
        private bool ignoreWalls;
        private float ghostTimer = 0;

        public Sprite Sprite
        {
            get { return this.sprite; }
        }

        public Bullet(Vector2 position, Vector2 direction, int bounce, float speed, Sprite sprite, Level level, bool ignoreWalls, float ghostTimer)
        {
            this.Position = position;
            this.Direction = direction;
            this.bounce = bounce;
            this.speed = speed;
            this.sprite = sprite;
            this.level = level;
            this.ignoreWalls = ignoreWalls;
            this.ghostTimer = ghostTimer;

            if (this.ignoreWalls)
            {
                this.sprite.Alpha = 0.5f;
            }
        }

        public void SwitchOwner(Player player)
        {
            this.sprite = new Sprite(player.BulletSprite);
            this.sprite.Effects.Add(new SpinEffect(200));
            this.sprite.Embellishments.Add(Embellishment.MakeGlow(player.BulletSpriteName, (float)player.BulletBounce / (Player.MaxBounce / 2.0f), false));
        }


        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.ignoreWalls)
            {
                this.ghostTimer -= elapsedTime;
                
                if (ghostTimer < 0)
                {
                    this.ignoreWalls = false;
                    this.sprite.Alpha = 1.0f;
                }
            }

            sprite.UpdateAnimation(gameTime);
            this.LastPosition = this.Position;
            this.Position += Direction * speed * elapsedTime;
            this.sprite.UpdateEffects(gameTime);
            this.sprite.UpdateAnimation(gameTime);
            this.CheckForWallCollisions();
        }

        private void CheckForWallCollisions()
        {
            if (this.ignoreWalls) return;

            xPenetrations.Clear();
            yPenetrations.Clear();
            List<Rectangle> possibleRectangles = level.GetPossibleRectangles(Position, LastPosition);

            foreach (Rectangle r in possibleRectangles)
            {
                Vector2 penetration = CollisionSolver.SolveCollision(this, r);
                if (penetration.X != 0)
                {
                    xPenetrations.Add(penetration.X);
                }
                if (penetration.Y != 0)
                {
                    yPenetrations.Add(penetration.Y);
                }
            }

            if (xPenetrations.Count != 0 || yPenetrations.Count != 0)
            {
                if (xPenetrations.Count >= yPenetrations.Count)
                {
                    this.Direction = new Vector2(-this.Direction.X, this.Direction.Y);
                }

                if (yPenetrations.Count >= xPenetrations.Count)
                {
                    this.Direction = new Vector2(this.Direction.X, -this.Direction.Y);
                }

                this.bounce--;

                if (this.bounce < 0)
                {
                    this.RemoveFromList = true;
                }
            }
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            sprite.Draw(batch, bounds.AdjustPoint(this.Position));
        }

        public Vector2 Position{get; set;}

        public Vector2 LastPosition { get; set; }

        public Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)(this.Position.X - (this.sprite.Width / 2.0f)), (int)(this.Position.Y - (this.sprite.Height / 2.0f)), this.sprite.Width, this.sprite.Height); }
        }

        public bool RemoveFromList { get; set; }
    }
}
