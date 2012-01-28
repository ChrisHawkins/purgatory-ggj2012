
namespace Purgatory.Game.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Animation;

    public class Embellishment
    {
        public bool Persists { get; set; }
        public SpriteEffect Entrance { get; set; }
        public SpriteEffect Exit { get; set; }
        public TimeSpan Lifespan { get; set; }
        public Sprite EmbellishmentSprite { get; set; }
        public Vector2 Offset { get; set; }
        private bool destroyed;

        private TimeSpan lifeSoFar;

        public void Update(GameTime gameTime)
        {
            this.EmbellishmentSprite.UpdateEffects(gameTime);
            this.EmbellishmentSprite.UpdateAnimation(gameTime);

            lifeSoFar += gameTime.ElapsedGameTime;

            if (lifeSoFar < Entrance.Duration)
            {
                this.Entrance.Update(EmbellishmentSprite, gameTime);
            }

            if (Persists & !destroyed) return;

            if (destroyed || (lifeSoFar > Lifespan - Exit.Duration))
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
            return !Persists && this.lifeSoFar > this.Lifespan;
        }

        public void Destroy()
        {
            this.destroyed = true;
        }
    }
}
