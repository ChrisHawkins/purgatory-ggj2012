
namespace Purgatory.Game.Animation
{
    using System;
    using Purgatory.Game.Graphics;

    public class PopInEffect : SpriteEffect
    {
        public float Exaggeration { get; private set; }

        private float timeToIncrease;
        private float timeToIncreaseMore;

        public PopInEffect(float milliseconds, float exaggeration)
        {
            this.Exaggeration = exaggeration;
            this.Duration = TimeSpan.FromMilliseconds(milliseconds);

            this.timeToIncrease = (milliseconds - (exaggeration * milliseconds)) / milliseconds;
            this.timeToIncreaseMore = timeToIncrease + ((exaggeration * milliseconds) / (milliseconds * 2));
        }

        public override void Update(Sprite sprite, float time)
        {
            if (time < this.timeToIncrease)
            {
                sprite.Zoom = time / timeToIncrease;
            }
            else if (time < this.timeToIncreaseMore)
            {
                sprite.Zoom = time / timeToIncrease;
            }
            else
            {
                sprite.Zoom = time;
            }
        }
    }
}
