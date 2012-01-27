
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Graphics;

    public class WinScreen: Screen
    {
        private ScreenManager screenManager;
        private SpriteBatch batch;
        private Sprite background;
        private float timer;

        public WinScreen(GraphicsDevice device)
            : base(device)
        {
            batch = new SpriteBatch(device);
        }

        public void SetBackground(Sprite sprite)
        {
            this.background = sprite;
            this.timer = 0.0f;
            //this.LoadScreen(this);
            this.CloseUntil(typeof(MainMenu));
        }

        public override void Draw(Bounds bounds)
        {
            batch.Begin();
            background.Draw(batch, new Vector2(background.Width, background.Height) / 2.0f);
            batch.End();
        }

        public override void Update(GameTime time)
        {
            timer += (float)time.ElapsedGameTime.TotalSeconds;

            if (this.timer > 3.0f)
            {
                this.CloseUntil(typeof(MainMenu));
            }
        }
    }
}
