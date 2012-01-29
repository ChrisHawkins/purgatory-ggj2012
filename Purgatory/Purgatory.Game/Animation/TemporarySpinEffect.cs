
namespace Purgatory.Game.Animation
{
    using System;
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Graphics;

    public class TemporarySpinEffect : SpriteEffect
    {
        public TemporarySpinEffect(float milliseconds)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
        }

        protected override void Update(Sprite sprite, float time)
        {
            sprite.Rotation = MathHelper.WrapAngle(time * MathHelper.TwoPi);
        }
    }
}
