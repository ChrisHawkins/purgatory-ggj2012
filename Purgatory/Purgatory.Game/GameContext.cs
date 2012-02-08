
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
        private const float PurgatoryTime = 30f;

        private Player player1;
        private Player player2;
        private float timeSinceLastRandomDropPlayerOne;
        private float timeSinceLastRandomDropPlayerTwo;
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

            Player.InputFrozen = false;
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

        public Level GetNormalLevel(PlayerNumber playerNumber)
        {
            if (playerNumber == PlayerNumber.PlayerOne)
            {
                return this.player1Level;
            }
            else
            {
                return this.player2Level;
            }
        }


        public void InitializePlayer(InputController playerOneControlScheme, InputController playerTwoControlScheme, ContentManager Content)
        {
            this.player1.Initialize(playerOneControlScheme, new DirectionalSprite("life"), "halo");
            this.player2.Initialize(playerTwoControlScheme, new DirectionalSprite("death"), "scythe");
        }

        public void UpdateGameLogic(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.player1.SetBulletDirection(player2.Position);
            this.player2.SetBulletDirection(player1.Position);
            this.player1.Update(gameTime);
            this.player2.Update(gameTime);

            if (!Player.InputFrozen)
            {
                this.player1.CheckBulletCollisions(player2.BulletList);
                this.player2.CheckBulletCollisions(player1.BulletList);
            }

            // Random item Drops
            if (!(player1.Level is PurgatoryLevel) && !(player2.Level is PurgatoryLevel))
            {
                this.timeSinceLastRandomDropPlayerOne += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.timeSinceLastRandomDropPlayerTwo += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.timeSinceLastRandomDropPlayerOne = this.UpdateRandomDrops(PlayerNumber.PlayerOne, this.timeSinceLastRandomDropPlayerOne, 1.5f);
                this.timeSinceLastRandomDropPlayerTwo = this.UpdateRandomDrops(PlayerNumber.PlayerTwo, this.timeSinceLastRandomDropPlayerTwo, 1.5f);
            }

            if (this.player1.Health < 1)
            {
                if (this.player1.Level is PurgatoryLevel)
                {
                    if (!this.player1.DeathSFX.IsPlaying)
                    {
                        Player.InputFrozen = true;
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
                    this.EnterPergatory(PlayerNumber.PlayerOne);
                }
            }

            if (this.player1.Level is PurgatoryLevel || this.player2.Level is PurgatoryLevel)
            {
                this.purgatoryTimer += elapsedTime;
            }

            // Check for purgatory and update the timer. Kill player if timer is up
            if (this.player1.Level is PurgatoryLevel)
            {
                if (purgatoryTimer >= GameContext.PurgatoryTime && !Player.InputFrozen)
                {
                    this.player1.Health = 0;
                    Player.InputFrozen = true;
                    AudioManager.Instance.FadeOut(this.purgatoryMusic, 1, true);
                    AudioManager.Instance.FadeOut(this.ds.BackgroundMusic, 1, true);
                    AudioManager.Instance.PlayCue(ref this.player1.DeathSFX, false);
                }
            }

            if (this.player2.Health < 1)
            {
                if (this.player2.Level is PurgatoryLevel)
                {
                    if (!this.player2.DeathSFX.IsPlaying)
                    {
                        Player.InputFrozen = true;
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
                    this.EnterPergatory(PlayerNumber.PlayerTwo);
                }
            }

            // Check for purgatory and update the timer. Kill player if timer is up
            if (this.player2.Level is PurgatoryLevel)
            {
                if (purgatoryTimer >= GameContext.PurgatoryTime && !Player.InputFrozen)
                {
                    this.player2.Health = 0;
                    Player.InputFrozen = true;
                    AudioManager.Instance.FadeOut(this.purgatoryMusic, 1, true);
                    AudioManager.Instance.FadeOut(this.ds.BackgroundMusic, 1, true);
                    AudioManager.Instance.PlayCue(ref this.player2.DeathSFX, false);
                }
            }
        }

        private float UpdateRandomDrops(PlayerNumber playerNumber, float timeSinceLastDrop, float timeBetweenDrops)
         {
            if (timeSinceLastDrop > timeBetweenDrops)
            {
                timeSinceLastDrop -= timeBetweenDrops;

                int num = rng.Next(70);
                int probability = 0;
        
                int chanceForHealthDrop = 5;
                int maxHealthDrops = 3;

                int chanceForShieldDrop = 8;
                int maxShieldDrops = 3;

                int chanceForSpiralDrop = 5;
                int maxSpiralDrops = 3;

                int chanceForNoClipDrop = 10;
                int maxNoClipDrops = 3;

                int chanceForBounceDrop = GetPlayer(playerNumber).BulletBounce / 2 < Player.MaxBounce ? Player.MaxBounce * 2 - (int)(GetPlayer(playerNumber).BulletBounce * 5.0f / 4.0f) : 0;
                int maxBounceDrops = 3;

                probability += chanceForHealthDrop;
                if (num < probability)
                {
                    if (GetNormalLevel(playerNumber).GetItemCount(typeof(HealthPickUp)) < maxHealthDrops)
                        GetNormalLevel(playerNumber).AddToPickups(new HealthPickUp(), false);
                    return timeSinceLastDrop - timeBetweenDrops;
                }

                probability += chanceForShieldDrop;
                if (num < probability)
                {
                    if (GetNormalLevel(playerNumber).GetItemCount(typeof(ShieldPowerUp)) < maxShieldDrops)
                        GetNormalLevel(playerNumber).AddToPickups(new ShieldPowerUp(), false);
                    return timeSinceLastDrop - timeBetweenDrops;;
                }

                probability += chanceForSpiralDrop;
                if (num < probability)
                {
                    if (GetNormalLevel(playerNumber).GetItemCount(typeof(SpiralShot)) < maxSpiralDrops)
                        GetNormalLevel(playerNumber).AddToPickups(new SpiralShot(), false);
                    return timeSinceLastDrop - timeBetweenDrops; ;
                }

                probability += chanceForNoClipDrop;
                if (num < probability)
                {
                    if (GetNormalLevel(playerNumber).GetItemCount(typeof(NoClipPowerUp)) < maxNoClipDrops)
                        GetNormalLevel(playerNumber).AddToPickups(new NoClipPowerUp(), false);
                    return timeSinceLastDrop - timeBetweenDrops; ;
                }

                probability += chanceForBounceDrop;
                if (num < probability)
                {
                    if (GetNormalLevel(playerNumber).GetItemCount(typeof(BouncePowerUp)) < maxBounceDrops)
                        GetNormalLevel(playerNumber).AddToPickups(new BouncePowerUp(), false);
                    return timeSinceLastDrop - timeBetweenDrops;
                }

                return timeSinceLastDrop - timeBetweenDrops;
            }
            return timeSinceLastDrop;
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

        private void EnterPergatory(PlayerNumber enteringPlayer)
        {
            this.purgatory.PlayPurgatoryAnimation();
            this.purgatoryTimer = 0f;
            Portal p = new Portal(this);
            p.EscapedPurgatory += new EventHandler(this.CrossfadePurgatoryToGameplay);
            this.purgatoryMusic = AudioManager.Instance.CrossFade(this.ds.BackgroundMusic, this.purgatoryMusic, 1.5f, false);
            this.purgatoryVoice = AudioManager.Instance.LoadCue(this.purgatoryVoice.Name);
            this.findPortalVoice = AudioManager.Instance.LoadCue(this.findPortalVoice.Name);
            this.purgatoryVoice = AudioManager.Instance.EnqueueCue(this.purgatoryVoice);
            this.findPortalVoice = AudioManager.Instance.EnqueueCue(this.findPortalVoice);
            this.player1.EnterPurgatory(enteringPlayer, purgatory, p);
            this.player2.EnterPurgatory(enteringPlayer, purgatory, p);
        }


    }
}
