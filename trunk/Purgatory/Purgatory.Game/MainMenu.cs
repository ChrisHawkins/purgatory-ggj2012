
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
using Purgatory.Game.Graphics;

    public class MainMenu : Screen
    {
        SpriteBatch batch;
        Sprite background;
        WinScreen winscreen;

        public MainMenu(GraphicsDevice device) : base(device)
        {
            background = new Sprite(BigEvilStatic.Content.Load<Texture2D>("title"), 1024, 768);
            batch = new SpriteBatch(device);
            this.winscreen = new WinScreen(device);
        }

        public override void Draw(Bounds bounds)
        {
            batch.Begin();
            this.Device.Clear(Color.Fuchsia);
            background.Draw(batch, new Vector2(background.Width, background.Height) / 2.0f);
            batch.End();
        }

        public override void Update(GameTime time)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.GetPressedKeys().Length > 0)
            {
                GameContext context = new GameContext(winscreen);
                context.InitializePlayer(BigEvilStatic.CreateControlSchemeWASD(), BigEvilStatic.CreateControlSchemeArrows(), BigEvilStatic.Content);

                DualScreen ds = new DualScreen(this.Device);
                ds.ContextUpdater = gt => context.UpdateGameLogic(gt);
                ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerOne));
                ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerTwo));

                this.LoadScreen(ds);
            }
        }
    }
}
