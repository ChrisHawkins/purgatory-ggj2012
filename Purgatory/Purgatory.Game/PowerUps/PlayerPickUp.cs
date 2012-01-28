
namespace Purgatory.Game.PowerUps
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Physics;

    public abstract class PlayerPickUp : IStatic
    {
        public Sprite Sprite { get; private set; }
        private Rectangle collisionRectangle;

        public PlayerPickUp(String spriteAsset)
        {
            Texture2D texture = BigEvilStatic.Content.Load<Texture2D>(spriteAsset);
            this.Sprite = new Sprite(texture, texture.Width, texture.Height);
        }

        public void SetPosition(Vector2 position)
        {
            this.Position = position;
            this.collisionRectangle = new Rectangle((int)(this.Position.X - (this.Sprite.Width / 2.0f)), (int)(this.Position.Y - (this.Sprite.Height / 2.0f)), this.Sprite.Width, this.Sprite.Height);
        }

        public abstract void PlayerEffect(Player player);

        public Microsoft.Xna.Framework.Vector2 Position { get; private set; }

        public Microsoft.Xna.Framework.Rectangle CollisionRectangle
        {
            get { return collisionRectangle; }
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.Sprite.Draw(batch, bounds.AdjustPoint(this.Position));
        }
    }
}
