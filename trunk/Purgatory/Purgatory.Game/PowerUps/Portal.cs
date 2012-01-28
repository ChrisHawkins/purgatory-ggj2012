using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Purgatory.Game.Animation;

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
            this.Sprite.Effects.Add(new SpinEffect(1000f));
            this.Sprite.Effects.Add(new GrowShrinkEffect(1000f, 0.2f));
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
