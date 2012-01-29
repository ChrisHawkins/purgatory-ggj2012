
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
        public string Name { get; private set; }
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

        public void Draw(SpriteBatch batch, Vector2 parentPosition, float parentZoom, float parentRotation)
        {
            this.EmbellishmentSprite.Zoom = parentZoom;
            this.EmbellishmentSprite.Rotation = parentRotation;
            this.EmbellishmentSprite.Draw(batch, parentPosition + Offset);
        }

        public bool HasFinished()
        {
            if (Persists) return destroyed;
            return this.lifeSoFar > this.Lifespan;
        }

        public void Destroy()
        {
            this.destroyed = true;
        }

        public static Embellishment MakeGlow(string sprite, float alpha)
        {
            Texture2D texture = BigEvilStatic.Content.Load<Texture2D>(sprite + "Glow");
            
            Embellishment embellishment = new Embellishment()
            {
                EmbellishmentSprite = new Sprite(texture, texture.Width, texture.Height),
                Entrance = new FadeEffect(500f, false, alpha),
                Exit = new FadeEffect(500f, true, alpha),
                Persists = true
            };

            embellishment.Name = "Glow";
            embellishment.EmbellishmentSprite.Alpha = 0f;

            var pulsate = new PulsateEffect(500f, 0.1f, alpha);
            pulsate.DelayStart(500f);
            embellishment.EmbellishmentSprite.Effects.Add(pulsate);
            return embellishment;
        }
    }
}
