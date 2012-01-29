using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Purgatory.Game.PowerUps
{
    public class BouncePowerUp : PlayerPickUp
    {
        public BouncePowerUp()
            : base("BouncePickUp")
        {
        }

        public override void PlayerEffect(Player player)
        {
            player.BulletBounce++;
            player.SetBounceGlow();
        }
    }
}
