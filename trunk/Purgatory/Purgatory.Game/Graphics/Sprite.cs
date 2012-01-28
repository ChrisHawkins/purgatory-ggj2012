
namespace Purgatory.Game.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Sprite
    {
        public bool PlayAnimation { get; set; }
        public int CurrentFrame { get; set; }
        public TimeSpan FrameTime { get; set; }
        public int FrameCount { get; private set; }
        public Color Tint { get; set; }
        public float Alpha { get; set; }
        public float Rotation { get; set; }

        public Texture2D Texture2D { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private TimeSpan sinceLastFrame = TimeSpan.Zero;
        private Sprite bulletSprite;

        public Sprite(Texture2D texture, int width, int height)
        {
            this.PlayAnimation = true;
            this.Texture2D = texture;
            this.Width = width;
            this.Height = height;
            this.Tint = Color.White;

            this.Alpha = 1f;

            this.LoadAnimation();
        }

        public Sprite(Sprite sprite) : this(sprite.Texture2D, sprite.Width, sprite.Height)
        {
        }

        private void LoadAnimation()
        {
            this.FrameCount = this.Texture2D.Width / this.Width;
            this.FrameTime = TimeSpan.FromMilliseconds(33.33);
        }

        public void UpdateAnimation(GameTime time)
        {
            if (this.PlayAnimation)
            {
                this.sinceLastFrame += time.ElapsedGameTime;

                if (this.sinceLastFrame > this.FrameTime)
                {
                    if (this.CurrentFrame < this.FrameCount - 1)
                    {
                        this.CurrentFrame++;
                    }
                    else
                    {
                        this.CurrentFrame = 0;
                    }

                    this.sinceLastFrame = TimeSpan.Zero;
                }
            }
            else
            {
                this.CurrentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 point, bool fromTopLeft = false)
        {
            Color multipliedTint = this.Tint * this.Alpha;

            spriteBatch.Draw(
                this.Texture2D,
                point,
                this.GetSourceRectangle(),
                multipliedTint,
                this.Rotation,
                this.GetOrigin(fromTopLeft),
                1.0f,
                SpriteEffects.None,
                0f);
        }

        private Vector2 GetOrigin(bool fromTopLeft)
        {
            if (fromTopLeft) return Vector2.Zero;
            else return new Vector2(this.Width / 2f, this.Height / 2f);
        }

        private Rectangle? GetSourceRectangle()
        {
            return new Rectangle(this.CurrentFrame * this.Width, 0, this.Width, this.Height);
        }
    }
}
