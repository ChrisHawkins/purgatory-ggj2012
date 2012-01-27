
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    public abstract class Screen
    {
        public Screen(GraphicsDevice device)
        {
            this.Device = device;
        }

        public GraphicsDevice Device { get; private set; }

        public abstract void Draw(Bounds bounds);
        public abstract void Update(GameTime time);

        protected void LoadScreen(Screen screen)
        {
            this.LoadingScreen(this, new ScreenEventArgs(screen));
        }

        protected void CloseScreen()
        {
            this.ClosingScreen(this, EventArgs.Empty);
        }

        public event EventHandler<ScreenEventArgs> LoadingScreen;
        public event EventHandler ClosingScreen;

        public virtual void OnControlReturned()
        {
        }
    }
}
