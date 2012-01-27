
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;
    using System.Collections.Generic;
    using Purgatory.Game.Physics;

    public class GameContext
    {
        private Player player1;
        private Player player2;

        private Level player1Level;
        private Level Player2Level;

        public GameContext()
        {
            player1 = new Player();
            player2 = new Player();

            this.player1Level = new Level("LifeMaze00");
            this.Player2Level = new Level("DeathMaze00");
        }

        public Player GetPlayer(PlayerNumber playerNumber)
        {
            if (playerNumber == PlayerNumber.PlayerOne)
            {
                return this.player1;
            }
            else if (playerNumber == PlayerNumber.PlayerTwo)
            {
                return this.player2;
            }
            else
            {
                return new Player();
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
                return this.Player2Level;
            }
            else
            {
                return null;
            }
        }

        public void InitializePlayer(KeyboardManager playerOneControlScheme, KeyboardManager playerTwoControlScheme, ContentManager Content)
        {
            Texture2D lifeTexture = Content.Load<Texture2D>("Player");
            Texture2D lifeBulletTexture = Content.Load<Texture2D>("Player");
            Texture2D deathTexture = Content.Load<Texture2D>("Death");
            Texture2D deathBulletTexture = Content.Load<Texture2D>("Death");

            this.player1.Initialize(playerOneControlScheme, new Graphics.Sprite(lifeTexture, lifeTexture.Height, lifeTexture.Height), new Graphics.Sprite(lifeBulletTexture, lifeBulletTexture.Height, lifeBulletTexture.Height));
            this.player2.Initialize(playerTwoControlScheme, new Graphics.Sprite(deathTexture, deathTexture.Height, deathTexture.Height), new Graphics.Sprite(deathBulletTexture, deathBulletTexture.Height, deathBulletTexture.Height));

            player1.Level = player1Level;
            player2.Level = Player2Level;
        }

        public void UpdateGameLogic(GameTime time)
        {
            this.player1.Update(time);
            this.player2.Update(time);

            this.player1.CheckBulletCollisions(player2.BulletList);
            this.player2.CheckBulletCollisions(player1.BulletList);

            if (this.player1.Health < 1)
            {
                //payer2 wins code here maybe
            }
            
            if (this.player2.Health < 0)
            {
                //player1 win code goes here
            }
        }
    }
}
