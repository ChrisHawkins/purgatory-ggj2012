
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

        public Texture2D Texture2D { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private TimeSpan sinceLastFrame = TimeSpan.Zero;

        public Sprite(Texture2D texture, int width, int height)
        {
            this.PlayAnimation = true;
            this.Texture2D = texture;
            this.Width = width;
            this.Height = height;

            this.LoadAnimation();
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
            if (fromTopLeft)
            {
                spriteBatch.Draw(
                    this.Texture2D,
                    point,
                    this.GetSourceRectangle(),
                    Color.White);
            }
            else
            {
                spriteBatch.Draw(
                    this.Texture2D,
                    this.GetDestinationRectangle(point),
                    this.GetSourceRectangle(),
                    Color.White);
            }
        }

        private Rectangle GetDestinationRectangle(Vector2 point)
        {
            return GeometryUtility.GetAdjustedRectangle(point, new Rectangle(0, 0, this.Width, this.Height));
        }

        private Rectangle? GetSourceRectangle()
        {
            return new Rectangle(this.CurrentFrame * this.Width, 0, this.Width, this.Height);
        }
    }
}
