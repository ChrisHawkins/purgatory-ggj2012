using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Purgatory.Game.Animation;

namespace Purgatory.Game.Graphics
{
    public class DirectionalSprite
    {
        private Sprite upSprite;
        private Sprite downSprite;
        private Sprite leftSprite;
        private Sprite rightSprite;

        public bool PlayAnimation { get; set; }

        public int Width {get { return upSprite.Width;}}
        public int Height {get { return upSprite.Height;}}

        public void AddEffect(SpriteEffect effect)
        {
            upSprite.Effects.Add(effect);
            downSprite.Effects.Add(effect);
            leftSprite.Effects.Add(effect);
            rightSprite.Effects.Add(effect);
        }

        public void UpdateAnimation(GameTime time)
        {
            upSprite.UpdateEffects(time);
            downSprite.UpdateEffects(time);
            leftSprite.UpdateEffects(time);
            rightSprite.UpdateEffects(time);

            if(this.PlayAnimation)
            {
                upSprite.UpdateAnimation(time);
                downSprite.UpdateAnimation(time);
                leftSprite.UpdateAnimation(time);
                rightSprite.UpdateAnimation(time);
            }
        }
        public DirectionalSprite(String spriteName)
        {
            Texture2D upTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Up");
            Texture2D downTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Down");
            Texture2D leftTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Left");
            Texture2D rightTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Right");
            this.upSprite = new Sprite(upTexture, upTexture.Width, upTexture.Height);
            this.downSprite = new Sprite(downTexture, downTexture.Width, upTexture.Height);
            this.leftSprite = new Sprite(leftTexture, leftTexture.Width, upTexture.Height);
            this.rightSprite = new Sprite(rightTexture, rightTexture.Width, upTexture.Height);
        }

        public void Draw(Vector2 direction, SpriteBatch batch, Vector2 point)
        {
             if(direction.Y > 0)
             {
                downSprite.Draw(batch, point);
                return;
             }

            if(direction.Y < 0)
             {
                upSprite.Draw(batch, point);
                return;
             }

            if(direction.X > 0)
             {
                rightSprite.Draw(batch, point);
                return;
             }

            if(direction.X < 0)
             {
                leftSprite.Draw(batch, point);
                return;
             }
        }

        public Sprite CreateSprite(Vector2 direction)
        {
            if (direction.Y > 0)
            {
                return new Sprite(downSprite);
            }
            else if (direction.Y < 0)
            {
                return new Sprite(upSprite);
            }
            else if (direction.X > 0)
            {
                return new Sprite(rightSprite);
            }
            else
            {
                return new Sprite(leftSprite);
            }
        }

    }
}
