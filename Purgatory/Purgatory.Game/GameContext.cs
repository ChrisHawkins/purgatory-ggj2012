
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Controls;

    public class GameContext
    {
        private Player player1;
        private Player player2;

        public GameContext()
        {
            player1 = new Player();
            player2 = new Player();
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

        public void InitializePlayer(KeyboardManager playerOneControlScheme, KeyboardManager playerTwoControlScheme, ContentManager Content)
        {
            this.player1.Initialize(playerOneControlScheme, new Graphics.Sprite(Content.Load<Texture2D>("death"), 64, 64));
            this.player2.Initialize(playerTwoControlScheme, new Graphics.Sprite(Content.Load<Texture2D>("Player"), 32, 32));
        }

        public void UpdateGameLogic(GameTime time)
        {
            this.player1.Update(time);
            this.player2.Update(time);
        }
    }
}
