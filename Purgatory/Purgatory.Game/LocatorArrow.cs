
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

        public Sprite Sprite { get; private set; }

        public LocatorArrow(Texture2D texture)
        {
            this.Sprite = new Sprite(texture, texture.Width, texture.Height);
            this.Sprite.Alpha = 1.0f;
        }

        public void Locate(Vector2 position1, Vector2 position2)
        {
            Vector2 diff = position2 - position1;
            float length = diff.Length();

            if (length > FadeOutDistance)
            {
                this.Sprite.Alpha = MathHelper.Clamp((length - FadeOutDistance) / FadeOverDistance, 0f, 1f);

                diff.Normalize();
                this.Sprite.Rotation = 2 * (float)Math.Atan2(diff.Y - 1, diff.X);
            }
            else
            {
                this.Sprite.Alpha = 0f;
            }
        }

        public void Draw(SpriteBatch batch, Vector2 playerPosition, Bounds bounds)
        {
            Sprite.Draw(batch, bounds.AdjustPoint(playerPosition));
        }
    }
}
