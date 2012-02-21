
namespace Purgatory.Game.Graphics
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Animation;
    using System.Collections.Generic;

    public class Embellishment
    {
        public bool Persists { get; set; }
        public SpriteEffect Entrance { get; set; }
        public SpriteEffect Exit { get; set; }
        public TimeSpan Lifespan { get; set; }
        public Sprite EmbellishmentSprite { get; set; }
        public Vector2 Offset { get; set; }
        public string Name { get; private set; }
        public bool Destroyed;

        private static Dictionary<string, Texture2D> textureMap = new Dictionary<string, Texture2D>();

        private TimeSpan lifeSoFar;

        public void Update(GameTime gameTime)
        {
            this.EmbellishmentSprite.UpdateEffects(gameTime);
            this.EmbellishmentSprite.UpdateAnimation(gameTime);

            lifeSoFar += gameTime.ElapsedGameTime;

            if (Entrance != null)
            {
                if (lifeSoFar < Entrance.Duration)
                {
                    this.Entrance.Update(EmbellishmentSprite, gameTime);
                }
            }

            if (Persists & !Destroyed) return;

            if (Exit != null)
            {
                if (Destroyed || (lifeSoFar > Lifespan - Exit.Duration))
                {
                    this.Exit.Update(EmbellishmentSprite, gameTime);
                }
            }
        }

        public void Draw(SpriteBatch batch, Vector2 parentPosition, float parentZoom, float parentRotation)
        {
            if (!Destroyed)
            {
                this.EmbellishmentSprite.Zoom = parentZoom;
                this.EmbellishmentSprite.Rotation = parentRotation;
                this.EmbellishmentSprite.Draw(batch, parentPosition + Offset);
            }
            else
            {
                int x = 3;
            }
        }

        public bool HasFinished()
        {
            if (Persists) return Destroyed;
            return this.lifeSoFar > this.Lifespan;
        }

        public void Destroy()
        {
            this.Destroyed = true;
        }

        public static Embellishment MakeGlow(string sprite, float alpha, bool fadeIn = true)
        {
            string textureName = sprite + "Glow";
            Texture2D texture = null;
            if (textureMap.ContainsKey(textureName))
            {
                texture = textureMap[textureName];
            }
            else
            {
                texture = BigEvilStatic.Content.Load<Texture2D>(textureName);
                textureMap.Add(textureName, texture);
            }

            Embellishment embellishment = new Embellishment()
            {
                EmbellishmentSprite = new Sprite(texture, texture.Width, texture.Height),
                Entrance = fadeIn ? new FadeEffect(500f, false, alpha) : null,
                Exit = new FadeEffect(500f, true, alpha),
                Persists = true
            };

            embellishment.Name = "Glow";
            embellishment.EmbellishmentSprite.Alpha = 0f;

            var pulsate = new PulsateEffect(500f, 0.1f, alpha);
            if (fadeIn) pulsate.DelayStart(500f);
            embellishment.EmbellishmentSprite.Effects.Add(pulsate);
            return embellishment;
        }
    }
}
