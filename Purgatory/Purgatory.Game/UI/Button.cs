
namespace Purgatory.Game.UI
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Graphics;

    public class Button : Control
    {
        private Sprite HoverState { get; set; }
        private Sprite NormalState { get; set; }

        public Action Action { get; set; }

        public Button()
        {
            this.TabStop = true;
        }

        public override void Update(GameTime time)
        {
            KeyboardState kb = Keyboard.GetState();

            if (this.HasFocus && (kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.Enter)))
            {
                this.Action();
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            if (!this.Visible) return;

            if (this.HasFocus)
            {
                this.HoverState.Draw(batch, this.Position);
            }
            else
            {
                this.NormalState.Draw(batch, this.Position);
            }
        }
    }
}
