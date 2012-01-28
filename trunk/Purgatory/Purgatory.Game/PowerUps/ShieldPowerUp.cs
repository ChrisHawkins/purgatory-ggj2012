using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Purgatory.Game.PowerUps
{
    public class ShieldPowerUp : PlayerPickUp
    {
        public ShieldPowerUp()
            : base("ShieldPickUp")
        { }

        public override void PlayerEffect(Player player)
        {
            player.ShieldHealth = 10;
        }
    }
}
