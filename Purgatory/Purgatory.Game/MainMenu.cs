
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
using Purgatory.Game.Graphics;
using Microsoft.Xna.Framework.Audio;

    public class MainMenu : Screen
    {
        private float timer;
        SpriteBatch batch;
        Sprite background;
        bool MusicPlaying = false;
        private Cue menuMusic;
        private Cue buttonPress;
        WinScreen winscreen;

        public MainMenu(GraphicsDevice device) : base(device)
        {
            background = new Sprite(BigEvilStatic.Content.Load<Texture2D>("title"), 1024, 768);
            batch = new SpriteBatch(device);
            this.winscreen = new WinScreen(device);
            menuMusic = AudioManager.Instance.LoadCue("Purgatory_MainMenu");
            buttonPress = AudioManager.Instance.LoadCue("Purgatory_ButtonPress");
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
            // play the menu music
            if (!MusicPlaying)
            {
                AudioManager.Instance.PlayCue(ref menuMusic, false);
                this.MusicPlaying = true;
            }

            KeyboardState kb = Keyboard.GetState();
            timer += (float)time.ElapsedGameTime.TotalSeconds;

            if (kb.GetPressedKeys().Length > 0 && timer > 0.5f)
            {
                this.timer = 0;
                DualScreen ds = new DualScreen(this.Device);
                GameContext context = new GameContext(ds, winscreen);
                context.InitializePlayer(BigEvilStatic.CreateControlSchemeWASD(), BigEvilStatic.CreateControlSchemeArrows(), BigEvilStatic.Content);

                ds.ContextUpdater = gt => context.UpdateGameLogic(gt);
                ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerOne));
                ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerTwo));

                this.LoadScreen(ds);

                AudioManager.Instance.PlayCue(ref this.buttonPress, false);

                // crossfade into game music
                AudioManager.Instance.CrossFade(this.menuMusic, ds.BackgroundMusic, 2f, true);
                this.MusicPlaying = false;
            }
        }
    }
}
