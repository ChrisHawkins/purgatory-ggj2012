
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

        public StatusBar(Vector2 position, float maxHealth, Texture2D texture)
        {
            this.Position = position;
            this.Maximum = maxHealth;
            Texture2D healthTexture = texture;
            this.sprite = new Sprite(healthTexture, healthTexture.Width, healthTexture.Height);
            this.originalWidth = healthTexture.Width;
        }

        public override void Update(GameTime time)
        {
            sprite.Width = (int)(originalWidth * this.Value / this.Maximum);
        }

        public override void Draw(SpriteBatch batch)
        {
            this.sprite.Draw(batch, new Vector2(Left, 0f) + Position, true);
        }
    }
}
