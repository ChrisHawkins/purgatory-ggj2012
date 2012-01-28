
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;

    public class ScreenManager
    {
        private Stack<Screen> screenStack = new Stack<Screen>();

        public void OpenScreen(Screen screen)
        {
            if (this.screenStack.Count > 0)
            {
                this.screenStack.First().OnControlLost();
            }

            screen.ClosingScreen += new EventHandler(ScreenClosing);
            screen.LoadingScreen += new EventHandler<ScreenEventArgs>(LoadingScreen);
            screen.ClosingAllScreensUntil += new EventHandler<ScreenTypeEventArgs>(CloseScreensUntil);
            this.screenStack.Push(screen);
        }

        void LoadingScreen(object sender, ScreenEventArgs e)
        {
            this.OpenScreen(e.Screen);
        }

        void ScreenClosing(object sender, EventArgs e)
        {
            this.screenStack.Pop();

            if (this.screenStack.Count == 0)
            {
                this.ScreensEmpty(this, EventArgs.Empty);
            }
            else
            {
                this.screenStack.First().OnControlReturned();
            }
        }

        void CloseScreensUntil(object sender, ScreenTypeEventArgs e)
        {
            while (this.screenStack.First().GetType() != e.ScreenType)
            {
                this.ScreenClosing(sender, e);
            }
        }

        public void Draw()
        {
            this.screenStack.First().Draw(Bounds.Screen);
        }

        public void Update(GameTime gameTime)
        {
            this.screenStack.First().Update(gameTime);
            AudioManager.Instance.Update(gameTime);
        }

        public event EventHandler ScreensEmpty;
    }
}
