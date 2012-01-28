
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.UI;

    public class GameScreen : Screen
    {
        private SpriteBatch batch;
        private GameContext context;
        private Texture2D texture;
        private PlayerNumber playerNumber;

        private Form hud;
        private StatusBar healthBar;
        private StatusBar energyBar;
        private LocatorArrow arrow;

        public GameScreen(GraphicsDevice device, GameContext context, PlayerNumber playerNumber)
            : base(device)
        {
            this.playerNumber = playerNumber;
            this.context = context;

            if (playerNumber == PlayerNumber.PlayerOne)
            {
                texture = BigEvilStatic.Content.Load<Texture2D>("TotalBlue");
            }
            else
            {
                texture = BigEvilStatic.Content.Load<Texture2D>("TotalRed");
            }

            this.batch = new SpriteBatch(device);

            this.hud = new Form(this.Device);

            this.healthBar = new StatusBar(
                new Vector2(10f, this.Device.Viewport.Height - 40f),
                this.context.GetPlayer(this.playerNumber).Health,
                BigEvilStatic.Content.Load<Texture2D>("HealthBar"));

            this.energyBar = new StatusBar(
                new Vector2(10f, this.Device.Viewport.Height - 60f),
                this.context.GetPlayer(this.playerNumber).Energy,
                BigEvilStatic.Content.Load<Texture2D>("EnergyBar"));

            this.arrow = new LocatorArrow(BigEvilStatic.Content.Load<Texture2D>("Arrow"));

            this.hud.Controls.Add(this.healthBar);
            this.hud.Controls.Add(this.energyBar);
        }

        public override void Draw(Bounds bounds)
        {
            RasterizerState state = new RasterizerState()
            {
                ScissorTestEnable = true
            };

            this.Device.ScissorRectangle = bounds.ToRectangle(this.Device);
            bounds.Camera = -this.context.GetPlayer(this.playerNumber).Position;

            this.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, state);

            this.batch.Draw(texture, this.Device.Viewport.Bounds, Color.White);

            this.context.GetLevel(playerNumber).Draw(batch, bounds);

            this.context.GetPlayer(PlayerNumber.PlayerOne).Draw(batch, bounds);
            this.context.GetPlayer(PlayerNumber.PlayerTwo).Draw(batch, bounds);

            this.arrow.Draw(this.batch, this.context.GetPlayer(this.playerNumber).Position, bounds);

            this.batch.End();

            this.healthBar.Left = bounds.Rectangle.Left;
            this.energyBar.Left = bounds.Rectangle.Left;
            this.hud.Draw();
        }

        public override void Update(GameTime time)
        {
            Vector2 pos1 = this.context.GetPlayer(PlayerNumber.PlayerOne).Position;
            Vector2 pos2 = this.context.GetPlayer(PlayerNumber.PlayerTwo).Position;

            if (this.playerNumber == PlayerNumber.PlayerOne)
            {
                this.arrow.Locate(pos1, pos2);
            }
            else
            {
                this.arrow.Locate(pos2, pos1);
            }

            this.healthBar.Value = this.context.GetPlayer(this.playerNumber).Health;
            this.energyBar.Value = this.context.GetPlayer(this.playerNumber).Energy;
            this.hud.Update(time);
        }
    }
}
