
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using System.Collections.Generic;
    using Purgatory.Game.Physics;
using System;

    public class GameContext
    {
        private const int RandomChanceForEnergyDrop = 1000;

        private Player player1;
        private Player player2;
        private WinScreen winScreen;
        private Level player1Level;
        private Level player2Level;
        private Random rng;

        public GameContext(WinScreen winScreen)
        {
            this.Time = 100;

            this.winScreen = winScreen;
            player1 = new Player(PlayerNumber.PlayerOne);
            player2 = new Player(PlayerNumber.PlayerTwo);

            this.player1Level = new Level("Life");
            this.player2Level = new Level("Death");

            player1.Level = player1Level;
            player2.Level = player2Level;

            rng = new Random();
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
                return this.player1Level;
            }
            else if (playerNumber == PlayerNumber.PlayerTwo)
            {
                return this.player2Level;
            }
            else
            {
                return null;
            }
        }

        public void InitializePlayer(KeyboardManager playerOneControlScheme, KeyboardManager playerTwoControlScheme, ContentManager Content)
        {
            Texture2D lifeTexture = Content.Load<Texture2D>("lifeDown");
            Texture2D lifeBulletTexture = Content.Load<Texture2D>("halo");
            Texture2D deathTexture = Content.Load<Texture2D>("deathDown");
            Texture2D deathBulletTexture = Content.Load<Texture2D>("deathDown");

            this.player1.Initialize(playerOneControlScheme, new Graphics.Sprite(lifeTexture, lifeTexture.Height, lifeTexture.Height), new Graphics.Sprite(lifeBulletTexture, lifeBulletTexture.Height, lifeBulletTexture.Height));
            this.player2.Initialize(playerTwoControlScheme, new Graphics.Sprite(deathTexture, deathTexture.Height, deathTexture.Height), new Graphics.Sprite(deathBulletTexture, deathBulletTexture.Height, deathBulletTexture.Height));


        }

        public void UpdateGameLogic(GameTime time)
        {
            this.Time -= (float)time.ElapsedGameTime.TotalSeconds;

            this.player1.SetBulletDirection(player2.Position);
            this.player2.SetBulletDirection(player1.Position);
            this.player1.Update(time);
            this.player2.Update(time);

            if (!Player.InputFrozen)
            {
                this.player1.CheckBulletCollisions(player2.BulletList);
                this.player2.CheckBulletCollisions(player1.BulletList);
            }

            // Random Ammo Drops
            if (rng.Next(RandomChanceForEnergyDrop) == 1)
            {
                //create ammo pack in life level
                int maxX = player1Level.WalkableTile.Length;
                int maxY = player1Level.WalkableTile[0].Length;
                
                bool ammoPlaced = false;
                while (!ammoPlaced)
                {
                    int locX = rng.Next(maxX);
                    int locY = rng.Next(maxY);

                    if (player1Level.WalkableTile[locX][locY] == TileType.Ground)
                    {
                        ammoPlaced = true;
                        this.player1Level.AddToPickups(new AmmoPickUp(new Vector2(locX, locY) * Level.TileWidth));
                    }
                }

                //create ammo pack in Death level
                maxX = player2Level.WalkableTile.Length;
                maxY = player2Level.WalkableTile[0].Length;

                ammoPlaced = false;
                while (!ammoPlaced)
                {
                    int locX = rng.Next(maxX);
                    int locY = rng.Next(maxY);

                    if (player2Level.WalkableTile[locX][locY] == TileType.Ground)
                    {
                        ammoPlaced = true;
                        this.player2Level.AddToPickups(new AmmoPickUp(new Vector2(locX, locY) * Level.TileWidth + new Vector2(Level.TileWidth) / 2));
                    }
                }
            }

            if (this.player1.Health < 1 && !this.player1.DeathSFX.IsPlaying)
            {
                Player.InputFrozen = false;
                winScreen.SetBackground(BigEvilStatic.CreateDeathWinBackground());
                winScreen.WinMusic = AudioManager.Instance.LoadCue("Purgatory_DeathWins");
                BigEvilStatic.ScreenManager.OpenScreen(winScreen);
                AudioManager.Instance.PlayCue(ref winScreen.WinMusic, false);
            }
            
            if (this.player2.Health < 1 && !this.player2.DeathSFX.IsPlaying)
            {
                //player1 win code goes here
                Player.InputFrozen = false;
                winScreen.SetBackground(BigEvilStatic.CreateLifeWinBackground());
                winScreen.WinMusic = AudioManager.Instance.LoadCue("Purgatory_LifeWins");
                BigEvilStatic.ScreenManager.OpenScreen(winScreen);
                AudioManager.Instance.PlayCue(ref winScreen.WinMusic, false);
            }
        }

        public float Time { get; private set; }
    }
}
