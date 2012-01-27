
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
            screen.ClosingScreen += new EventHandler(ScreenClosing);
            screen.LoadingScreen += new EventHandler<ScreenEventArgs>(LoadingScreen);
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

        public void Draw()
        {
            this.screenStack.First().Draw(Bounds.Screen);
        }

        public void Update(GameTime time)
        {
            this.screenStack.First().Update(time);
        }

        public event EventHandler ScreensEmpty;
    }
}
