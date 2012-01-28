
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Controls;
    using Purgatory.Game.Graphics;

    public static class BigEvilStatic
    {
        public static ScreenManager ScreenManager { get; private set; }

        public static ContentManager Content { get; private set; }

        public static Viewport Viewport { get; private set; }

        public static void Init(ScreenManager screenManager, ContentManager manager, Viewport viewport)
        {
            ScreenManager = screenManager;
            Content = manager;
            Viewport = viewport;
        }

        public static KeyboardInputController CreateControlSchemeWASD()
        {
            return new KeyboardInputController(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.LeftShift);
        }

        public static KeyboardInputController CreateControlSchemeArrows()
        {
            return new KeyboardInputController(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter, Keys.RightShift);
        }

        public static KeyboardInputController CreateControlWinatronPlayerOne()
        {
            return new KeyboardInputController(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Z, Keys.X); // use X aswell
        }

        public static KeyboardInputController CreateControlWinatronPlayerTwo()
        {
            return new KeyboardInputController(Keys.I, Keys.K, Keys.J, Keys.L, Keys.M, Keys.M); // can use n aswell
        }

        public static Sprite CreateDeathWinBackground()
        {
            return new Sprite(Content.Load<Texture2D>("deathwins"), 1024, 768);
        }

        public static Sprite CreateLifeWinBackground()
        {
            return new Sprite(Content.Load<Texture2D>("lifewins"), 1024, 768);
        }
    }
}
