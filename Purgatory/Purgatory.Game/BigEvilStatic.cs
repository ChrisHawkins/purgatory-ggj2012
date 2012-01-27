
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
using Purgatory.Game.Controls;
    using Microsoft.Xna.Framework.Input;

    public static class BigEvilStatic
    {
        public static ContentManager Content { get; private set; }

        public static Viewport Viewport { get; private set; }

        public static void Init(ContentManager manager, Viewport viewport)
        {
            Content = manager;
            Viewport = viewport;
        }

        public static KeyboardManager CreateControlSchemeWASD()
        {
            return new KeyboardManager(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space);
        }

        public static KeyboardManager CreateControlSchemeArrows()
        {
            return new KeyboardManager(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter);
        }

        public static KeyboardManager CreateControlWinatronPlayerOne()
        {
            return new KeyboardManager(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Z); // use X aswell
        }

        public static KeyboardManager CreateControlWinatronPlayerTwo()
        {
            return new KeyboardManager(Keys.I, Keys.K, Keys.J, Keys.L, Keys.M); // can use n aswell
        }
    }
}
