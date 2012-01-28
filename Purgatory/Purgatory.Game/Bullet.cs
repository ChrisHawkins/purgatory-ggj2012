
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Purgatory.Game.Physics;
    using Microsoft.Xna.Framework;
using Purgatory.Game.Graphics;
    using Microsoft.Xna.Framework.Graphics;

    public class Bullet : IMoveable
    {
        public Vector2 Direction { get; set; }
        private int bounce;
        private float speed;
        private Sprite sprite;
        private Level level;
        private List<float> xPenetrations;
        private List<float> yPenetrations;

        public Sprite Sprite
        {
            get { return this.sprite; }
        }

        public Bullet(Vector2 position, Vector2 direction, int bounce, float speed, Sprite sprite, Level level)
        {
            this.Position = position;
            this.Direction = direction;
            this.bounce = bounce;
            this.speed = speed;
            this.sprite = sprite;
            this.level = level;
            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();
        }

        public void SwitchOwner(Player player)
        {
            this.sprite = new Sprite(player.BulletSprite);
            //this.level = player.Level;
        }


        public void Update(GameTime time)
        {
            sprite.UpdateAnimation(time);
            this.LastPosition = this.Position;
            this.Position += Direction * speed * (float)time.ElapsedGameTime.TotalSeconds;
            this.sprite.UpdateEffects(time);
            this.sprite.UpdateAnimation(time);
            this.CheckForCollisions();

        }

        private void CheckForCollisions()
        {
            this.xPenetrations.Clear();
            this.yPenetrations.Clear();
            List<Rectangle> possibleRectangles = level.GetPossibleRectangles(Position, LastPosition);

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
