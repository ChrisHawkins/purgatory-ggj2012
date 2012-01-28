
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;

    public class LocatorArrow
    {
        public Sprite Sprite { get; private set; }

        public LocatorArrow(Texture2D texture)
        {
            this.Sprite = new Sprite(texture, texture.Width, texture.Height);
        }

        public void Locate(Vector2 position1, Vector2 position2)
        {
            Vector2 diff = position2 - position1;
            float length = diff.Length();

            if (length > 300)
            {
                this.Sprite.Alpha = MathHelper.Clamp((length - 300) / 100f, 0f, 1f);

                //this.Sprite.Rotation
            }
            else
            {
                this.Sprite.Alpha = 0f;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            Sprite.Draw(batch, new Vector2(0f, 0f));
        }
    }
}
