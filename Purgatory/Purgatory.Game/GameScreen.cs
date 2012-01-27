
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

        public GameScreen(GraphicsDevice device, GameContext context) : base(device)
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

            this.batch = new SpriteBatch(device);
        }

        public override void Draw(Bounds bounds)
        {
            this.batch.Begin();
            this.batch.Draw(texture, bounds.ToRectangle(this.Device), Color.White);
            this.batch.End();
        }

        public static bool FIRST_ASSIGNED_DELETE_THIS;
    }
}
