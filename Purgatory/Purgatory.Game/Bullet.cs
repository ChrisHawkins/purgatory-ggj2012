
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
        private Vector2 direction;
        private float speed;
        private Sprite sprite;

        public Bullet(Vector2 position, Vector2 direction, float speed, Sprite sprite)
        {
            this.Position = position;
            this.direction = direction;
            this.speed = speed;
            this.sprite = sprite;
        }

        public void Update(GameTime time)
        {
            this.LastPosition = this.Position;
            this.Position += direction * speed * (float)time.ElapsedGameTime.TotalSeconds;
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
