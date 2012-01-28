using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Purgatory.Game.Graphics
{
    public class DashSprite
    {
        private Sprite sprite;
        private Vector2 position;
        public bool RemoveFromList { get; private set; }

        public DashSprite(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.sprite.Alpha = 0.8f;
            this.position = position;
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            sprite.Draw(batch, bounds.AdjustPoint(this.position));
        }

        public void update(GameTime time)
        {
            this.sprite.Alpha -= 2.0f * (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}
