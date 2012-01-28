
namespace Purgatory.Game.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Animation;
using Microsoft.Xna.Framework.Graphics;

    public class Embellishment
    {
        public bool Persists { get; set; }
        public SpriteEffect Entrance { get; set; }
        public SpriteEffect Exit { get; set; }
        public TimeSpan Lifespan { get; set; }
        public Sprite EmbellishmentSprite { get; set; }
        public Vector2 Offset { get; set; }

        private TimeSpan lifeSoFar;

        public void Update(GameTime gameTime)
        {
            lifeSoFar += gameTime.ElapsedGameTime;

            if (lifeSoFar < Entrance.Duration)
            {
                this.Entrance.Update(EmbellishmentSprite, gameTime);
            }

            if (Persists) return;

            if (lifeSoFar > Lifespan - Exit.Duration)
            {
                this.Exit.Update(EmbellishmentSprite, gameTime);
            }
        }

        public void Draw(SpriteBatch batch, Vector2 parentPosition)
        {
            this.EmbellishmentSprite.Draw(batch, parentPosition + Offset);
        }

        public bool HasFinished()
        {
            return this.lifeSoFar > this.Lifespan;
        }
    }
}
