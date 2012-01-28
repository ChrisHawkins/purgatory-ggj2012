
namespace Purgatory.Game.Graphics
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Animation;

    public class Sprite
    {
        public bool PlayAnimation { get; set; }
        public int CurrentFrame { get; set; }
        public TimeSpan FrameTime { get; set; }
        public int FrameCount { get; private set; }
        public Color Tint { get; set; }
        public float Alpha { get; set; }
        public float Rotation { get; set; }
        public List<SpriteEffect> Effects { get; private set; }
        public List<Embellishment> Embellishments { get; private set; }
        public float Zoom { get; set; }
        public Texture2D Texture2D { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private TimeSpan sinceLastFrame = TimeSpan.Zero;

        public Sprite(Texture2D texture, int width, int height)
        {
            this.PlayAnimation = true;
            this.Texture2D = texture;
            this.Width = width;
            this.Height = height;
            this.Tint = Color.White;
            this.Effects = new List<SpriteEffect>();
            this.Embellishments = new List<Embellishment>();
            this.Zoom = 1.0f;

            this.Alpha = 1f;

            this.LoadAnimation();
        }

        public Sprite(Sprite sprite) : this(sprite.Texture2D, sprite.Width, sprite.Height)
        {
            this.CurrentFrame = sprite.CurrentFrame;
        }

        private void LoadAnimation()
        {
            this.FrameCount = this.Texture2D.Width / this.Width;
            this.FrameTime = TimeSpan.FromMilliseconds(33.33);
        }

        public void UpdateEffects(GameTime time)
        {
            for (int i = Effects.Count - 1; i >= 0; --i)
            {
                Effects[i].Update(this, time);

                if (Effects[i].HasFinished())
                {
                    Effects.RemoveAt(i);
                }
            }

            for (int i = this.Embellishments.Count - 1; i >= 0; --i)
            {
                this.Embellishments[i].Update(time);

                if (this.Embellishments[i].HasFinished())
                {
                    this.Embellishments.RemoveAt(i);
                }
            }
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
                this.Zoom,
                SpriteEffects.None,
                0f);

            for (int i = this.Embellishments.Count - 1; i >= 0; --i)
            {
                this.Embellishments[i].Draw(spriteBatch, point, this.Zoom);
            }
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
