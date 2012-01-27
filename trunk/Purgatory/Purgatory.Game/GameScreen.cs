
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;

    public class GameScreen : Screen
    {
        private SpriteBatch batch;
        private GameContext context;
        private Texture2D texture;
        private PlayerNumber playerNumber;
        private Level level;

        public GameScreen(GraphicsDevice device, GameContext context, PlayerNumber playerNumber) : base(device)
        {
            this.playerNumber = playerNumber;
            this.context = context;
            this.level = this.context.GetPlayer(playerNumber).Level;

            if (playerNumber == PlayerNumber.PlayerOne)
            {
                texture = BigEvilStatic.Content.Load<Texture2D>("TotalBlue");
            }
            else
            {
                texture = BigEvilStatic.Content.Load<Texture2D>("TotalRed");
            }

            this.batch = new SpriteBatch(device);
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
            this.level.Draw(batch, bounds);

            this.context.GetPlayer(PlayerNumber.PlayerOne).Draw(batch, bounds);
            this.context.GetPlayer(PlayerNumber.PlayerTwo).Draw(batch, bounds);

            this.batch.End();
        }

        public override void Update(GameTime time)
        {
        }
    }
}
