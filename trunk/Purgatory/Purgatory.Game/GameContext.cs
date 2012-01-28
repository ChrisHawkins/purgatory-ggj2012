
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using System.Collections.Generic;
    using Purgatory.Game.Physics;
    using System;
    using Purgatory.Game.Graphics;
    using Microsoft.Xna.Framework.Audio;

    public class GameContext
    {
        private const int RandomChanceForEnergyDrop = 1000;

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
        bool endFadingMusic = false;

        public GameContext(DualScreen ds, WinScreen winScreen)
        {
            this.ds = ds;
            this.Time = 100;

            this.winScreen = winScreen;
            player1 = new Player(PlayerNumber.PlayerOne);
            player2 = new Player(PlayerNumber.PlayerTwo);

            this.player1Level = LevelGenerator.GenerateLevel("Life");
            this.player2Level = LevelGenerator.GenerateLevel("Death");
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

        public void InitializePlayer(KeyboardManager playerOneControlScheme, KeyboardManager playerTwoControlScheme, ContentManager Content)
        {
            Texture2D lifeBulletTexture = Content.Load<Texture2D>("halo");
            Texture2D deathBulletTexture = Content.Load<Texture2D>("scythe");

            this.player1.Initialize(playerOneControlScheme, new DirectionalSprite("life"), new Graphics.Sprite(lifeBulletTexture, lifeBulletTexture.Height, lifeBulletTexture.Height));
            this.player2.Initialize(playerTwoControlScheme, new DirectionalSprite("death"), new Graphics.Sprite(deathBulletTexture, deathBulletTexture.Height, deathBulletTexture.Height));


        }

        public void UpdateGameLogic(GameTime time)
        {
            this.Time -= (float)time.ElapsedGameTime.TotalSeconds;
            this.timeSinceLastRandomDrop += (float)time.ElapsedGameTime.TotalSeconds;

            this.player1.SetBulletDirection(player2.Position);
            this.player2.SetBulletDirection(player1.Position);
            this.player1.Update(time);
            this.player2.Update(time);

            if (!Player.InputFrozen)
            {
                this.player1.CheckBulletCollisions(player2.BulletList);
                this.player2.CheckBulletCollisions(player1.BulletList);
            }

            // Random Energy Drops
            
            if (timeSinceLastRandomDrop > 1)
            {
                timeSinceLastRandomDrop -= 1;
                player1Level.AddToPickups(new AmmoPickUp());
                player2Level.AddToPickups(new AmmoPickUp());
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
                    this.player1.Level = purgatory;
                    AudioManager.Instance.CrossFade(ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
                    this.player1.Health = 10;
                    
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
                    this.player2.Level = purgatory;
                    AudioManager.Instance.CrossFade(ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
                    this.player2.Health = 10;
                }
            }
        }

        public float Time { get; private set; }
    }
}
