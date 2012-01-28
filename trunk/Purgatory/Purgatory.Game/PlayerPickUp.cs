using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Purgatory.Game.Physics;
using Purgatory.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Purgatory.Game
{
    public abstract class PlayerPickUp : IStatic
    {
        private Sprite sprite;
        private Rectangle collisionRectangle;

        public PlayerPickUp(String spriteAsset)
        {
            Texture2D texture = BigEvilStatic.Content.Load<Texture2D>(spriteAsset);
            this.sprite = new Sprite(texture, texture.Width, texture.Height);
        }

        public void SetPosition(Vector2 position)
        {
            this.Position = position;
            this.collisionRectangle = new Rectangle((int)(this.Position.X - (this.sprite.Width / 2.0f)), (int)(this.Position.Y - (this.sprite.Height / 2.0f)), this.sprite.Width, this.sprite.Height);
        }

        public abstract void PlayerEffect(Player player);

        public Microsoft.Xna.Framework.Vector2 Position { get; private set; }

        public Microsoft.Xna.Framework.Rectangle CollisionRectangle
        {
            get { return collisionRectangle;}
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            sprite.Draw(batch, bounds.AdjustPoint(this.Position));
        }
    }
}
