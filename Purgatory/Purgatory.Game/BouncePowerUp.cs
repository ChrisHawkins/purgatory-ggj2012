using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Purgatory.Game
{
    public class BouncePowerUp : PlayerPickUp
    {
        public BouncePowerUp()
            : base("BlackWall")
        {
        }

        public override void PlayerEffect(Player player)
        {
            player.bulletBounce++;
        }
    }
}
