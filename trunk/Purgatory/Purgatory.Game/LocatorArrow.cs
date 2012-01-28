
namespace Purgatory.Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;

    public class LocatorArrow
    {
        private const int FadeOutDistance = 300;
        private const float FadeOverDistance = 100;

        public Sprite RegularSprite { get; private set; }
        public Sprite PurgatorySprite { get; private set; }

        public bool Purgatory { get; set; }

        public LocatorArrow(Texture2D regularTexture, Texture2D purgatoryTexture)
        {
            this.RegularSprite = new Sprite(regularTexture, regularTexture.Width, regularTexture.Height);
            this.RegularSprite.Alpha = 1.0f;

            this.PurgatorySprite = new Sprite(purgatoryTexture, purgatoryTexture.Width, purgatoryTexture.Height);
            this.PurgatorySprite.Alpha = 1.0f;
        }

        public void Locate(Vector2 position1, Vector2 position2)
        {
            Vector2 diff = position2 - position1;
            float length = diff.Length();

            if (length > FadeOutDistance)
            {
                if (this.Purgatory)
                {
                    this.PurgatorySprite.Alpha = MathHelper.Clamp((length - FadeOutDistance) / FadeOverDistance, 0f, 1f);
                    diff.Normalize();
                    this.PurgatorySprite.Rotation = 2 * (float)Math.Atan2(diff.Y - 1, diff.X);
                }
                else
                {
                    this.RegularSprite.Alpha = MathHelper.Clamp((length - FadeOutDistance) / FadeOverDistance, 0f, 1f);
                    diff.Normalize();
                    this.RegularSprite.Rotation = 2 * (float)Math.Atan2(diff.Y - 1, diff.X);
                }
            }
            else
            {
                this.RegularSprite.Alpha = 0f;
            }
        }

        public void Draw(SpriteBatch batch, Vector2 playerPosition, Bounds bounds)
        {
            if (this.Purgatory)
            {
                PurgatorySprite.Draw(batch, bounds.AdjustPoint(playerPosition));
            }
            else
            {
                RegularSprite.Draw(batch, bounds.AdjustPoint(playerPosition));
            }
        }
    }
}
