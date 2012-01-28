
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.UI;

    public class StatusBar : Control
    {
        private Sprite sprite;
        private int originalWidth;

        public float Maximum { get;set;}
        public float Value { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public bool RightToLeft { get; set; }

        public StatusBar(Vector2 position, float maxHealth, Texture2D texture)
        {
            this.Position = position;
            this.Maximum = maxHealth;
            Texture2D healthTexture = texture;
            this.sprite = new Sprite(healthTexture, healthTexture.Width, healthTexture.Height);
            this.originalWidth = healthTexture.Width;
        }

        private int CalculateWidth()
        {
            return (int)(originalWidth * this.Value / this.Maximum);
        }

        public override void Update(GameTime time)
        {
            sprite.Width = this.CalculateWidth();
        }

        public override void Draw(SpriteBatch batch)
        {
            if (this.RightToLeft)
            {
                this.sprite.Draw(batch, new Vector2(batch.GraphicsDevice.Viewport.Width - Right - sprite.Width, 0f) + Position, true);
            }
            else
            {
                this.sprite.Draw(batch, new Vector2(Left, 0f) + Position, true);
            }
        }
    }
}
