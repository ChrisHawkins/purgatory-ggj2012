using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Purgatory.Game.Graphics
{
    public class DirectionalSprite
    {
        Sprite upSprite;
        Sprite downSprite;
        Sprite leftSprite;
        Sprite rightSprite;

        public DirectionalSprite(String spriteName)
        {
            Texture2D upTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Up");
            Texture2D downTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Down");
            Texture2D leftTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Left");
            Texture2D rightTexture = BigEvilStatic.Content.Load<Texture2D>(spriteName + "Right");
            this.upSprite = new Sprite(upTexture, upTexture.Height, upTexture.Height);
            this.downSprite = new Sprite(downTexture, upTexture.Height, upTexture.Height);
            this.leftSprite = new Sprite(leftTexture, upTexture.Height, upTexture.Height);
            this.rightSprite = new Sprite(rightTexture, upTexture.Height, upTexture.Height);
        }
    }
}
