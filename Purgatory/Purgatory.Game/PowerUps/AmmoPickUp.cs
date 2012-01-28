using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Purgatory.Game.PowerUps
{
    public class AmmoPickUp : PlayerPickUp
    {
        public AmmoPickUp()
            : base("EnergyPickUp")
        { }

        public override void PlayerEffect(Player player)
        {
            player.Energy = 100;
        }
    }
}
