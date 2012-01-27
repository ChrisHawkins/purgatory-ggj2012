
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
        private Player player;

        public GameScreen(GraphicsDevice device, GameContext context)
            : base(device)
        {
            this.context = context;

            if (!FIRST_ASSIGNED_DELETE_THIS)
            {
                texture = BigEvilStatic.Content.Load<Texture2D>("TotalBlue");
                FIRST_ASSIGNED_DELETE_THIS = true;
            }
            else
            {
                texture = BigEvilStatic.Content.Load<Texture2D>("TotalRed");
            }

            Sprite playerSprite = new Sprite(BigEvilStatic.Content.Load<Texture2D>("death"), 32, 32);
            this.player = new Player(playerSprite, BigEvilStatic.CreateControlSchemeArrows());
            this.batch = new SpriteBatch(device);
        }

        public override void Update(GameTime time)
        {
            this.player.Update(time);
        }

        public override void Draw(Bounds bounds)
        {
            RasterizerState state = new RasterizerState()
            {
                ScissorTestEnable = true
            };

            this.Device.ScissorRectangle = bounds.ToRectangle(this.Device);

            this.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, state);

            this.batch.Draw(texture, this.Device.Viewport.Bounds, Color.White);
            this.player.Draw(batch, bounds);

            this.batch.End();
        }

        public static bool FIRST_ASSIGNED_DELETE_THIS;
    }
}
