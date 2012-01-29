
namespace Purgatory.Game.Animation
{
    using System;
    using Purgatory.Game.Graphics;

    public class FadeEffect : SpriteEffect
    {
        private bool fadeOut;
        private float alpha;

        public FadeEffect(float milliseconds, bool fadeOut, float targetAlpha = 1.0f)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.fadeOut = fadeOut;
            this.alpha = targetAlpha;
        }

        protected override void Update(Sprite sprite, float time)
        {
            if (fadeOut) time = 1.0f - time;
            time *= alpha;
            sprite.Alpha = time;
        }
    }
}
