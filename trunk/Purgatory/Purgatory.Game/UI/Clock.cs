
namespace Purgatory.Game.UI
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Clock : Control
    {
        public int Value { get; set; }

        public Clock()
        {
            NumberFont.LoadFont(string.Empty);
        }

        public override void Update(GameTime time)
        {
            // does nothing
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!this.Visible)
            {
                return;
            }

            float width = NumberFont.CalculateWidth(this.Value);
            Vector2 position = this.Position - new Vector2(width / 2f, 0f);
            NumberFont.DrawNumber(batch, position, this.Value);
        }
    }
}
