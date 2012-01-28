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
        public Cue PurgatoryMusic;
        public Cue BackgroundMusic;

        public Portal(Level playerLevel, Cue purgatoryMusic, Cue backgroundMusic)
            : base("Portal")
        {
            this.PlayerLevel = playerLevel;
            this.PurgatoryMusic = purgatoryMusic;
            this.BackgroundMusic = backgroundMusic;
        }

        public override void PlayerEffect(Player player)
        {
            player.Level = this.PlayerLevel;
            PlayerLevel.PlayPurgatoryAnimation();
            player.Spawn();
            AudioManager.Instance.CrossFade(this.PurgatoryMusic, this.BackgroundMusic, 1.5f, true);
        }
    }
}
