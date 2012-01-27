
namespace Purgatory.Game.UI
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Form
    {
        private SpriteBatch batch;
        public bool Visible { get; set; }

        public Form(GraphicsDevice device)
        {
            this.Controls = new List<Control>();
            this.batch = new SpriteBatch(device);
            this.Visible = true;
        }

        public List<Control> Controls { get; private set; }

        public void Draw()
        {
            if (this.Visible)
            {
                this.batch.Begin();

                foreach (var control in this.Controls)
                {
                    control.Draw(batch);
                }

                this.batch.End();
            }
        }

        public void Update(GameTime time)
        {
            foreach (var control in this.Controls)
            {
                control.Update(time);
            }
        }
    }
}
