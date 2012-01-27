
namespace Purgatory.Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Screen
    {
        public Screen(GraphicsDevice device)
        {
            this.Device = device;
        }

        public GraphicsDevice Device { get; private set; }

        public abstract void Draw(Bounds bounds);
        public abstract void Update(GameTime time);

        public void LoadScreen(Screen screen)
        {
            if (LoadingScreen != null)
            {
                this.LoadingScreen(this, new ScreenEventArgs(screen));
            }
        }

        protected void CloseScreen()
        {
            this.ClosingScreen(this, EventArgs.Empty);
        }

        protected void CloseUntil(Type screenType)
        {
            this.ClosingAllScreensUntil(this, new ScreenTypeEventArgs(screenType));
        }

        public event EventHandler<ScreenEventArgs> LoadingScreen;
        public event EventHandler ClosingScreen;
        public event EventHandler<ScreenTypeEventArgs> ClosingAllScreensUntil;

        public virtual void OnControlReturned()
        {
        }
    }
}
