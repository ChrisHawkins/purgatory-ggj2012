using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Purgatory.Game.PowerUps
{
    public class HealthPickUp : PlayerPickUp
    {
        public HealthPickUp()
            : base("HealthPickUp")
        { }

        public override void PlayerEffect(Player player)
        {
            player.Health += 30;
        }
    }
}
