
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

    public class DualScreen : Screen
    {
        public DualScreen(GraphicsDevice device) : base(device)
        {
            this.screens = new List<ScreenToDraw>();
        }

        private List<ScreenToDraw> screens;

        public override void Draw(Bounds bounds)
        {
            foreach (var toDraw in screens)
            {
                toDraw.Screen.Draw(toDraw.Bounds);
            }
        }

        private struct ScreenToDraw
        {
            public Screen Screen;
            public Bounds Bounds;

            public ScreenToDraw(Screen screen, Rectangle bounds)
            {
                Screen = screen;
                Bounds = new Bounds(bounds);
            }
        }
    }
}
