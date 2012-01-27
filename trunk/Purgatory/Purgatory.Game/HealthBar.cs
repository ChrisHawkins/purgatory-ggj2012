using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Purgatory.Game.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Purgatory.Game
{
    public class HealthBar
    {
        public Sprite Sprite;
        public float MaxHealth;
        public float Health;
        public Vector2 Position;
        private int originalWidth;

        public HealthBar(Vector2 position, float maxHealth)
        {
            this.Position = position;
            this.MaxHealth = maxHealth;
            Texture2D healthTexture = BigEvilStatic.Content.Load<Texture2D>("HealthBar");
            this.Sprite = new Sprite(healthTexture, healthTexture.Width, healthTexture.Height);
            this.originalWidth = healthTexture.Width;
        }

        public void Update(float playerHealth)
        {
            this.Health = playerHealth;
            Sprite.Width = (int)(originalWidth * this.Health / this.MaxHealth);
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.Sprite.Draw(batch, Position);
        }
    }
}
