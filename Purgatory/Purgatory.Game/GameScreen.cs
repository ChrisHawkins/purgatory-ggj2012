
namespace Purgatory.Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class GameScreen : Screen
    {
        private SpriteBatch batch;
        private GameContext context;
        private Texture2D texture;
        private Player player;
        private Texture2D playerSprite;

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

            this.player = new Player();
            this.playerSprite = BigEvilStatic.Content.Load<Texture2D>("Player");
            this.batch = new SpriteBatch(device);
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
            this.batch.Draw(playerSprite, bounds.AdjustPoint(this.player.Position), Color.White);

            this.batch.End();
        }


        public static bool FIRST_ASSIGNED_DELETE_THIS;

        public override void Update(GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}
