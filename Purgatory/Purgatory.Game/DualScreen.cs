
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class DualScreen : Screen
    {
        private List<ScreenToDraw> screens;

        public DualScreen(GraphicsDevice device) : base(device)
        {
            this.screens = new List<ScreenToDraw>();
        }

        public override void Draw(Bounds bounds)
        {
            foreach (var toDraw in screens)
            {
                toDraw.Screen.Draw(toDraw.Bounds);
            }
        }

        public void AddScreen(Screen screen)
        {
            this.screens.Add(new ScreenToDraw(screen));

            foreach (var screenInList in this.screens)
            {
                screenInList.Bounds = this.GetBounds(screenInList.Screen);
            }
        }

        private Bounds GetBounds(Screen screen)
        {
            if (this.screens.Count > 2)
            {
                throw new InvalidOperationException("Max 2 screens");
            }

            // VERTICALLY

            //int heightAlloc = Device.Viewport.Height / 2;

            //int i;

            //for (i = 0; i < this.screens.Count; i++)
            //{
            //    if (this.screens[i].Screen == screen) break;
            //}

            //return new Bounds(new Rectangle(0, heightAlloc * i, this.Device.Viewport.Width, heightAlloc));

            // HORIZONTALLY

            int widthAlloc = Device.Viewport.Width / 2;

            int i;

            for (i = 0; i < this.screens.Count; i++)
            {
                if (this.screens[i].Screen == screen) break;
            }

            return new Bounds(new Rectangle(i * widthAlloc, 0, widthAlloc, this.Device.Viewport.Height));
        }

        public override void Update(GameTime time)
        {
            this.ContextUpdater(time);

            foreach (var screen in this.screens)
            {
                screen.Screen.Update(time);
            }
        }

        public Action<GameTime> ContextUpdater { get; set; }

        private class ScreenToDraw
        {
            public Screen Screen;
            public Bounds Bounds;

            public ScreenToDraw(Screen screen)
            {
                Screen = screen;
                Bounds = Bounds.Screen;
            }
        }
    }
}
