using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Purgatory.Game.PowerUps
{
    public class SpiralShot : PlayerPickUp
    {
        public SpiralShot()
            : base("EnergyPickUp")
        {
        }

        public override void PlayerEffect(Player player)
        {
            player.TimeSinceSpiralBegan = 0;
        }
    }
}
