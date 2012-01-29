
namespace Purgatory.Game.Animation
{
    using System;
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Graphics;

    public class TemporarySpinEffect : SpriteEffect
    {
        private float startRotation;

        public TemporarySpinEffect(float milliseconds, float startRotation)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.startRotation = startRotation;
        }

        protected override void Update(Sprite sprite, float time)
        {
            sprite.Rotation = MathHelper.WrapAngle(startRotation + time * MathHelper.TwoPi);
        }
    }
}
