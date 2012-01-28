using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Purgatory.Game
{
    public class AmmoPickUp : PlayerPickUp
    {
        public AmmoPickUp()
            : base("BlackWall")
        { }

        public override void PlayerEffect(Player player)
        {
            player.Energy = 100;
        }
    }
}
