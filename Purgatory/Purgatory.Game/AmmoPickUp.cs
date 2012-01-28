using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Purgatory.Game
{
    public class AmmoPickUp : PlayerPickUp
    {
        public AmmoPickUp(Vector2 position)
            : base("BlackWall", position)
        { }

        public override void PlayerEffect(Player player)
        {
            player.Energy = 100;
        }
    }
}
