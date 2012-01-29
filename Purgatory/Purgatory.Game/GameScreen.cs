
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

        private StatusBar healthBar;
        private StatusBar energyBar;
        //private StatusBar timeBar;
        private Clock clock;
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

            this.healthBar = new StatusBar(
                new Vector2(120f, this.Device.Viewport.Height - 65f),
                this.context.GetPlayer(this.playerNumber).Health,
                BigEvilStatic.Content.Load<Texture2D>("HealthBar"));

            this.energyBar = new StatusBar(
                new Vector2(120f, this.Device.Viewport.Height - 30f),
                this.context.GetPlayer(this.playerNumber).Energy,
                BigEvilStatic.Content.Load<Texture2D>("EnergyBar"));

            //this.timeBar = new StatusBar(
            //    new Vector2(0f, 10f),
            //    this.context.PurgatoryCountdown,
            //    BigEvilStatic.Content.Load<Texture2D>("EnergyBar"));
            this.clock = new Clock();

            //this.timeBar.Visible = false;
            this.clock.Visible = false;

            //if (this.playerNumber == PlayerNumber.PlayerOne) this.timeBar.RightToLeft = true;

            this.arrow = new LocatorArrow(BigEvilStatic.Content.Load<Texture2D>("ArrowRed"), BigEvilStatic.Content.Load<Texture2D>("ArrowGreen"));

            this.Hud.Controls.Add(this.healthBar);
            this.Hud.Controls.Add(this.energyBar);
            //this.Hud.Controls.Add(this.timeBar);
            this.Hud.Controls.Add(this.clock);
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

            this.clock.Position = new Vector2(bounds.Rectangle.Left + bounds.Rectangle.Width / 2f, 4f);
            //this.timeBar.Left = bounds.Rectangle.Left;
            //this.timeBar.Right = bounds.Rectangle.Right;

            this.Hud.Draw();
        }

        public override void Update(GameTime time)
        {
            Vector2 pos1 = this.context.GetPlayer(PlayerNumber.PlayerOne).Position;
            Vector2 pos2 = this.context.GetPlayer(PlayerNumber.PlayerTwo).Position;

            Player player = this.context.GetPlayer(this.playerNumber);

            this.arrow.Purgatory = false;

            if (player.Level is PurgatoryLevel)
            {
                this.arrow.Purgatory = true;
                this.arrow.Locate(player.Position, (player.Level as PurgatoryLevel).Portal.Position);
            }
            else if (this.playerNumber == PlayerNumber.PlayerOne)
            {
                this.arrow.Locate(pos1, pos2);
            }
            else
            {
                this.arrow.Locate(pos2, pos1);
            }

            this.healthBar.Value = this.context.GetPlayer(this.playerNumber).Health;
            this.energyBar.Value = this.context.GetPlayer(this.playerNumber).Energy;

            //this.timeBar.Value = this.context.PurgatoryCountdown;
            //this.timeBar.Visible = this.context.InPurgatory;

            this.clock.Value = (int)this.context.PurgatoryCountdown;
            this.clock.Visible = this.context.InPurgatory;

            this.Hud.Update(time);
        }
    }
}
