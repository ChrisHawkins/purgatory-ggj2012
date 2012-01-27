
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class MainMenu : Screen
    {
        public MainMenu(GraphicsDevice device) : base(device)
        {
        }

        public override void Draw(Bounds bounds)
        {
            this.Device.Clear(Color.Fuchsia);
        }

        public override void Update(GameTime time)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Enter) || kb.IsKeyDown(Keys.Space))
            {
                GameContext context = new GameContext();
                context.InitializePlayer(BigEvilStatic.CreateControlSchemeWASD(), BigEvilStatic.CreateControlSchemeArrows(), BigEvilStatic.Content);

                DualScreen ds = new DualScreen(this.Device);
                ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerOne));
                ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerTwo));

                this.LoadScreen(ds);
            }
        }
    }
}
