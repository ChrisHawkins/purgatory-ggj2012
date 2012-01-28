﻿
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
        private const float PurgatoryTime = 30f;

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
        private Cue purgatoryVoice;
        private Cue findPortalVoice;
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

            Random rand = new Random();
            int mapNo = rand.Next(3);
            int pairNo = rand.Next(2);

            this.player1Level = LevelGenerator.GenerateLevelFromTexture("Life", mapNo, pairNo);
            this.player2Level = LevelGenerator.GenerateLevelFromTexture("Death", mapNo, 1 - pairNo);
            this.purgatory = new PurgatoryLevel("Purgatory", this.player1Level.WalkableTile, this.player2Level.WalkableTile);

            player1.Level = player1Level;
            player2.Level = player2Level;

            rng = new Random();
            this.purgatoryMusic = AudioManager.Instance.LoadCue("Purgatory_PurgatoryChase");
            this.purgatoryVoice = AudioManager.Instance.LoadCue("Purgatory_Purgatory");
            this.findPortalVoice = AudioManager.Instance.LoadCue("Purgatory_FindPortal");
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
                timeSinceLastRandomDrop -= 3;

                int num = rng.Next(100);

                if (num < 15)
                {
                    if (player1Level.GetItemCount(typeof(BouncePowerUp)) < 4 && player1.BulletBounce < Player.MaxBounce && !(player1.Level is PurgatoryLevel))
                        player1Level.AddToPickups(new BouncePowerUp());
                    if (player2Level.GetItemCount(typeof(BouncePowerUp)) < 4 && player2.BulletBounce < Player.MaxBounce && !(player2.Level is PurgatoryLevel))
                        player2Level.AddToPickups(new BouncePowerUp()); 
                }
                else if (num < 25)
                {
                    if (player1Level.GetItemCount(typeof(HealthPickUp)) < 4 && player1.Health < Player.MaxHealth && !(player1.Level is PurgatoryLevel))
                        player1Level.AddToPickups(new HealthPickUp());
                    if (player2Level.GetItemCount(typeof(HealthPickUp)) < 4 && player2.Health < Player.MaxHealth && !(player2.Level is PurgatoryLevel))
                        player2Level.AddToPickups(new HealthPickUp());
                }
                else if (num < 30)
                {
                    if (player1Level.GetItemCount(typeof(ShieldPowerUp)) < 1 && !(player1.Level is PurgatoryLevel))
                        this.player1Level.AddToPickups(new ShieldPowerUp());
                    if (player2Level.GetItemCount(typeof(ShieldPowerUp)) < 1 && !(player2.Level is PurgatoryLevel))
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
                    Portal p = new Portal(this.player1Level);
                    p.EscapedPurgatory += new EventHandler(this.CrossfadePurgatoryToGameplay);
                    purgatory.AddToPickups(p, this.player1.Position, 50 * 32);
                    this.purgatoryMusic = AudioManager.Instance.CrossFade(this.ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
                    this.purgatoryVoice = AudioManager.Instance.LoadCue(this.purgatoryVoice.Name);
                    this.findPortalVoice = AudioManager.Instance.LoadCue(this.findPortalVoice.Name);
                    this.purgatoryVoice = AudioManager.Instance.EnqueueCue(this.purgatoryVoice);
                    this.findPortalVoice = AudioManager.Instance.EnqueueCue(this.findPortalVoice);
                    this.player1.EnterPurgatory(PlayerNumber.PlayerOne);
                    this.player2.EnterPurgatory(PlayerNumber.PlayerOne);
                }
            }

            // Check for purgatory and update the timer. Revive player if timer is up
            if (this.player1.Level is PurgatoryLevel)
            {
                this.purgatoryTimer += elapsedTime;

                if (purgatoryTimer >= GameContext.PurgatoryTime)
                {
                    this.player1.Health = 0;
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
                    Portal p = new Portal(this.player2Level);
                    p.EscapedPurgatory += new EventHandler(this.CrossfadePurgatoryToGameplay);
                    purgatory.AddToPickups(p, this.player2.Position, 50 * 32);
                    this.purgatoryMusic = AudioManager.Instance.CrossFade(this.ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
                    this.purgatoryVoice = AudioManager.Instance.LoadCue(this.purgatoryVoice.Name);
                    this.findPortalVoice = AudioManager.Instance.LoadCue(this.findPortalVoice.Name);
                    this.purgatoryVoice = AudioManager.Instance.EnqueueCue(this.purgatoryVoice);
                    this.findPortalVoice = AudioManager.Instance.EnqueueCue(this.findPortalVoice);
                    this.player1.EnterPurgatory(PlayerNumber.PlayerTwo);
                    this.player2.EnterPurgatory(PlayerNumber.PlayerTwo);
                }
            }

            // Check for purgatory and update the timer. Revive player if timer is up
            if (this.player2.Level is PurgatoryLevel)
            {
                this.purgatoryTimer += elapsedTime;

                if (purgatoryTimer >= GameContext.PurgatoryTime)
                {
                    this.player2.Health = 0;
                }
            }
        }

        public void CrossfadePurgatoryToGameplay(object sender, EventArgs e)
        {
            this.ds.BackgroundMusic = AudioManager.Instance.CrossFade(this.purgatoryMusic, this.ds.BackgroundMusic, 1.5f, true);
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
