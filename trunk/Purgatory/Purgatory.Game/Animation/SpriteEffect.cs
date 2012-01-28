
namespace Purgatory.Game.Animation
{
    using System;
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Graphics;

    public abstract class SpriteEffect
    {
        public TimeSpan Duration { get; protected set; }
        protected TimeSpan TimeElapsed { get; private set; }

        protected abstract void Update(Sprite sprite, float time);

        public void Update(Sprite sprite, GameTime gameTime)
        {
            this.TimeElapsed += gameTime.ElapsedGameTime;
            this.Update(sprite, (float)(this.TimeElapsed.TotalMilliseconds / this.Duration.TotalMilliseconds));
        }

        public bool HasFinished()
        {
            return this.TimeElapsed > this.Duration;
        }
    }
}
