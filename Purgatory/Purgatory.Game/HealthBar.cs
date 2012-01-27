
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.UI;

    public class HealthBar : Control
    {
        private Sprite sprite;
        private int originalWidth;

        public float MaxHealth { get;set;}
        public float Health { get; set; }
        public Vector2 Position { get; set; }
        public int Left { get; set; }

        public HealthBar(Vector2 position, float maxHealth)
        {
            this.Position = position;
            this.MaxHealth = maxHealth;
            Texture2D healthTexture = BigEvilStatic.Content.Load<Texture2D>("HealthBar");
            this.sprite = new Sprite(healthTexture, healthTexture.Width, healthTexture.Height);
            this.originalWidth = healthTexture.Width;
        }

        public override void Update(GameTime time)
        {
            sprite.Width = (int)(originalWidth * this.Health / this.MaxHealth);
        }

        public override void Draw(SpriteBatch batch)
        {
            this.sprite.Draw(batch, new Vector2(Left, this.sprite.Texture2D.GraphicsDevice.Viewport.Height - 40f) + Position, true);
        }
    }
}
