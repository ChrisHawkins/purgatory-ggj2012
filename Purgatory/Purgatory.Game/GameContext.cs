
namespace Purgatory.Game
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.PowerUps;

    public class GameContext
    {
        private const int RandomChanceForEnergyDrop = 1000;
        private const float PurgatoryTime = 20f;

        private float timeSinceLastRandomDrop;
        private Player player1;
        private Player player2;
        private WinScreen winScreen;
        private Level player1Level;
        private Level player2Level;
        private PurgatoryLevel purgatory;
        private Random rng;
        private DualScreen ds;
        private Cue purgatoryMusic;
        private bool endFadingMusic = false;
        private float purgatoryTimer;

        public float PurgatoryCountdown
        {
            get { return PurgatoryTime - this.purgatoryTimer; }
        }

        public GameContext(DualScreen ds, WinScreen winScreen)
        {
            this.ds = ds;

            this.winScreen = winScreen;
            player1 = new Player(PlayerNumber.PlayerOne);
            player2 = new Player(PlayerNumber.PlayerTwo);

            this.player1Level = LevelGenerator.GenerateLevelFromTexture("Life");
            this.player2Level = LevelGenerator.GenerateLevelFromTexture("Death");
            this.purgatory = new PurgatoryLevel("Death", this.player1Level.WalkableTile, this.player2Level.WalkableTile);

            player1.Level = player1Level;
            player2.Level = player2Level;

            rng = new Random();
            this.purgatoryMusic = AudioManager.Instance.LoadCue("Purgatory_PurgatoryChase");
        }

        public Player GetPlayer(PlayerNumber playerNumber)
        {
            if (playerNumber == PlayerNumber.PlayerOne)
            {
                return this.player1;
            }
            else
            {
                return this.player2;
            }
        }

        public Level GetLevel(PlayerNumber playerNumber)
        {
            if (playerNumber == PlayerNumber.PlayerOne)
            {
                return this.player1.Level;
            }
            else if (playerNumber == PlayerNumber.PlayerTwo)
            {
                return this.player2.Level;
            }
            else
            {
                return null;
            }
        }

        public void InitializePlayer(IInputController playerOneControlScheme, IInputController playerTwoControlScheme, ContentManager Content)
        {
            Texture2D lifeBulletTexture = Content.Load<Texture2D>("halo");
            Texture2D deathBulletTexture = Content.Load<Texture2D>("scythe");

            this.player1.Initialize(playerOneControlScheme, new DirectionalSprite("life"), new Graphics.Sprite(lifeBulletTexture, lifeBulletTexture.Height, lifeBulletTexture.Height));
            this.player2.Initialize(playerTwoControlScheme, new DirectionalSprite("death"), new Graphics.Sprite(deathBulletTexture, deathBulletTexture.Height, deathBulletTexture.Height));
        }

        public void UpdateGameLogic(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.timeSinceLastRandomDrop += elapsedTime;

            this.player1.SetBulletDirection(player2.Position);
            this.player2.SetBulletDirection(player1.Position);
            this.player1.Update(gameTime);
            this.player2.Update(gameTime);

            if (!Player.InputFrozen)
            {
                this.player1.CheckBulletCollisions(player2.BulletList);
                this.player2.CheckBulletCollisions(player1.BulletList);
            }

            // Random Energy Drops
            
            if (timeSinceLastRandomDrop > 3)
            {
                timeSinceLastRandomDrop -= 1;
                int num = rng.Next(100);

                if (num < 20)
                {
                    player1Level.AddToPickups(new BouncePowerUp());
                    player2Level.AddToPickups(new BouncePowerUp());
                }
                else if (num < 30)
                {
                    player1Level.AddToPickups(new HealthPickUp());
                    player2Level.AddToPickups(new HealthPickUp());
                }
                else if (num < 35)
                {
                    this.player1Level.AddToPickups(new ShieldPowerUp());
                    this.player2Level.AddToPickups(new ShieldPowerUp());
                }
            }

            if (this.player1.Health < 1)
            {
                if (this.player1.Level is PurgatoryLevel)
                {
                    if (!this.player1.DeathSFX.IsPlaying)
                    {
                        Player.InputFrozen = false;
                        winScreen.SetBackground(BigEvilStatic.CreateDeathWinBackground());
                        winScreen.WinMusic = AudioManager.Instance.LoadCue("Purgatory_DeathWins");
                        BigEvilStatic.ScreenManager.OpenScreen(winScreen);
                        AudioManager.Instance.PlayCue(ref winScreen.WinMusic, false);
                    }
                    else if (!endFadingMusic)
                    {
                        endFadingMusic = true;
                        AudioManager.Instance.FadeOut(this.purgatoryMusic, 1, true);
                        AudioManager.Instance.FadeOut(this.ds.BackgroundMusic, 1, true);
                    }
                }
                else
                {
                    this.purgatory.PlayPurgatoryAnimation();
                    this.player1.Level = purgatory;
                    this.purgatoryTimer = 0f;
                    AudioManager.Instance.CrossFade(ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
                    this.player1.Health = 10;
                }
            }

            // Check for purgatory and update the timer. Revive player if timer is up
            if (this.player1.Level is PurgatoryLevel)
            {
                this.purgatoryTimer += elapsedTime;

                if (purgatoryTimer >= GameContext.PurgatoryTime)
                {
                    this.player1.Level = this.player1Level;
                    this.player1.Spawn();
                    AudioManager.Instance.CrossFade(this.purgatoryMusic, this.ds.BackgroundMusic, 1.5f, true);
                }
            }
            
            if (this.player2.Health < 1)
            {
                if (this.player2.Level is PurgatoryLevel)
                {
                    if (!this.player2.DeathSFX.IsPlaying)
                    {
                        Player.InputFrozen = false;
                        winScreen.SetBackground(BigEvilStatic.CreateLifeWinBackground());
                        winScreen.WinMusic = AudioManager.Instance.LoadCue("Purgatory_LifeWins");
                        BigEvilStatic.ScreenManager.OpenScreen(winScreen);
                        AudioManager.Instance.PlayCue(ref winScreen.WinMusic, false);
                    }
                    else if (!this.endFadingMusic)
                    {
                        this.endFadingMusic = true;
                        AudioManager.Instance.FadeOut(this.purgatoryMusic, 1, true);
                        AudioManager.Instance.FadeOut(this.ds.BackgroundMusic, 1, true);
                    }
                }
                else
                {
                    this.purgatory.PlayPurgatoryAnimation();
                    this.player2.Level = purgatory;
                    this.purgatoryTimer = 0f;
                    AudioManager.Instance.CrossFade(ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
                    this.player2.Health = 10;
                }
            }

            // Check for purgatory and update the timer. Revive player if timer is up
            if (this.player2.Level is PurgatoryLevel)
            {
                this.purgatoryTimer += elapsedTime;

                if (purgatoryTimer >= GameContext.PurgatoryTime)
                {
                    this.player2.Level = this.player2Level;
                    this.player2.Spawn();
                    AudioManager.Instance.CrossFade(this.purgatoryMusic, this.ds.BackgroundMusic, 1.5f, true);
                }
            }
        }

        public bool InPurgatory
        {
            get
            {
                return this.player1.Level is PurgatoryLevel || this.player2.Level is PurgatoryLevel;
            }
        }
    }
}
