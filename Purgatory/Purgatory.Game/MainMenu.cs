
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Controls;
    using System;

    public class MainMenu : Screen
    {
        private float timer;
        SpriteBatch batch;
        Sprite backgroundPC;
        Sprite backgroundXBOX;
        bool MusicPlaying = false;
        private Cue menuMusic;
        private Cue buttonPress;
        private Cue beginVoice;
        WinScreen winscreen;

        public MainMenu(GraphicsDevice device)
            : base(device)
        {
            backgroundPC = new Sprite(BigEvilStatic.Content.Load<Texture2D>("titleKeyboard"), 1024, 768);
            backgroundXBOX = new Sprite(BigEvilStatic.Content.Load<Texture2D>("titleXbox"), 1024, 768);
            batch = new SpriteBatch(device);
            this.winscreen = new WinScreen(device);
            menuMusic = AudioManager.Instance.LoadCue("Purgatory_MainMenu");
            buttonPress = AudioManager.Instance.LoadCue("Purgatory_ButtonPress");
            this.beginVoice = AudioManager.Instance.LoadCue("Purgatory_Begin");
        }

        public override void Draw(Bounds bounds)
        {
            batch.Begin();
            this.Device.Clear(Color.Fuchsia);
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                backgroundXBOX.Draw(batch, new Vector2(backgroundXBOX.Width, backgroundXBOX.Height) / 2.0f);
            }
            else
            {
                backgroundPC.Draw(batch, new Vector2(backgroundPC.Width, backgroundPC.Height) / 2.0f);
            }
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
                this.StartGame(ds, context);
            }

            GamePadState gs = GamePad.GetState(PlayerIndex.One);

            if (XboxUtility.ButtonPressed(true))
            {
                this.timer = 0;
                DualScreen ds = new DualScreen(this.Device);
                GameContext context = new GameContext(ds, winscreen);

                context.InitializePlayer(BigEvilStatic.CreateControlXboxPlayerOne(), BigEvilStatic.CreateControlXboxPlayerTwo(), BigEvilStatic.Content);
                this.StartGame(ds, context);
            }
        }

        private void StartGame(DualScreen ds, GameContext context)
        {
            ds.ContextUpdater = gt => context.UpdateGameLogic(gt);
            ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerOne));
            ds.AddScreen(new GameScreen(this.Device, context, PlayerNumber.PlayerTwo));

            this.LoadScreen(ds);

            AudioManager.Instance.PlayCue(ref this.buttonPress, false);
            AudioManager.Instance.PlayCue(ref this.beginVoice, false);

            // crossfade into game music
            ds.BackgroundMusic = AudioManager.Instance.CrossFade(this.menuMusic, ds.BackgroundMusic, 2f, true);
            this.MusicPlaying = false;
        }

        public override void OnControlReturned()
        {
            base.OnControlReturned();
            GC.Collect();
        }
    }
}
