
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public class ScreenManager
    {
        private List<Screen> screenStack = new List<Screen>();

        public void OpenScreen(Screen screen)
        {
            if (this.screenStack.Count > 0)
            {
                this.screenStack[this.screenStack.Count - 1].OnControlLost();
            }

            screen.ClosingScreen += new EventHandler(ScreenClosing);
            screen.LoadingScreen += new EventHandler<ScreenEventArgs>(LoadingScreen);
            screen.ClosingAllScreensUntil += new EventHandler<ScreenTypeEventArgs>(CloseScreensUntil);
            this.screenStack.Add(screen);
        }

        void LoadingScreen(object sender, ScreenEventArgs e)
        {
            this.OpenScreen(e.Screen);
        }

        void ScreenClosing(object sender, EventArgs e)
        {
            this.screenStack.RemoveAt(this.screenStack.Count - 1);

            if (this.screenStack.Count == 0)
            {
                this.ScreensEmpty(this, EventArgs.Empty);
            }
            else
            {
                this.screenStack[this.screenStack.Count - 1].OnControlReturned();
            }
        }

        void CloseScreensUntil(object sender, ScreenTypeEventArgs e)
        {
            while (this.screenStack[this.screenStack.Count - 1].GetType() != e.ScreenType)
            {
                this.ScreenClosing(sender, e);
            }
        }

        public void Draw()
        {
            this.screenStack[this.screenStack.Count - 1].Draw(Bounds.Screen);
        }

        public void Update(GameTime gameTime)
        {
            this.screenStack[this.screenStack.Count - 1].Update(gameTime);
            AudioManager.Instance.Update(gameTime);
        }

        public event EventHandler ScreensEmpty;
    }
}
