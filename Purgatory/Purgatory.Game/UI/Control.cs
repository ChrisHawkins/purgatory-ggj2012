
namespace Purgatory.Game.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Control
    {
        public Control()
        {
            this.Visible = true;
        }

        public bool TabStop { get; protected set; }
        public bool HasFocus { get; set; }
        public Vector2 Position { get; set; }
        public bool Visible { get; set; }
        public abstract void Update(GameTime time);
        public abstract void Draw(SpriteBatch batch);
    }
}
