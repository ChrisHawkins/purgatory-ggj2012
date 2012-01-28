
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Controls;

    public class WinScreen: Screen
    {
        private ScreenManager screenManager;
        private SpriteBatch batch;
        private Sprite background;
        private float timer;
        private KeyboardState kb;
        public Cue WinMusic;
        
        public WinScreen(GraphicsDevice device)
            : base(device)
        {
            batch = new SpriteBatch(device);
        }

        public void SetBackground(Sprite sprite)
        {
            this.background = sprite;
            this.timer = 0.0f;
            //this.CloseUntil(typeof(MainMenu));
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

            if (this.timer > 1.0f)
            {
                kb = Keyboard.GetState();
                if (kb.GetPressedKeys().Length > 0 || XboxUtility.ButtonDown(true))
                {
                    this.CloseUntil(typeof(MainMenu));
                    Cue buttonPress = AudioManager.Instance.LoadCue("Purgatory_ButtonPress");
                    AudioManager.Instance.PlayCue(ref buttonPress, false);
                }
            }
        }

        public override void OnControlLost()
        {
            base.OnControlLost();

            AudioManager.Instance.FadeOut(this.WinMusic, 1, true);
        }
    }
}
