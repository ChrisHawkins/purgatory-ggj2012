
namespace Purgatory.Game.Animation
{
    using System;
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Graphics;

    public class PulsateEffect : SpriteEffect
    {
        private float rangeFromMainOpacity;

        public PulsateEffect(float milliseconds, float rangeFromMainOpacity)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.Permanent = true;

            this.rangeFromMainOpacity = rangeFromMainOpacity;
        }

        protected override void Update(Sprite sprite, float time)
        {
            if (time > 2.0f) this.ResetTime();
            if (time > 1.0f) time = 1.0f - (time - 1.0f);

            time -= 0.5f;
            time *= 2.0f;

            sprite.Alpha = MathHelper.Clamp(sprite.Alpha + rangeFromMainOpacity * time, 0f, 1f);
        }
    }
}
