
namespace Purgatory.Game.Animation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Purgatory.Game.Graphics;

    public class FadeEffect : SpriteEffect
    {
        private bool fadeOut;

        public FadeEffect(float milliseconds, bool fadeOut)
        {
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);
            this.fadeOut = fadeOut;
        }

        protected override void Update(Sprite sprite, float time)
        {
            if (fadeOut) time = 1.0f - time;
            sprite.Alpha = time;
        }
    }
}
