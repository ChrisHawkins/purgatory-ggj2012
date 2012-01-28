using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Purgatory.Game.PowerUps
{
    public class Portal : PlayerPickUp
    {
        public Level PlayerLevel;
        public event EventHandler EscapedPurgatory;

        public Portal(Level playerLevel)
            : base("Portal")
        {
            this.PlayerLevel = playerLevel;
        }

        public override void PlayerEffect(Player player)
        {
            player.Level = this.PlayerLevel;
            PlayerLevel.PlayPurgatoryAnimation();
            player.Spawn();
            this.EscapedPurgatory(this, EventArgs.Empty);
        }
    }
}
