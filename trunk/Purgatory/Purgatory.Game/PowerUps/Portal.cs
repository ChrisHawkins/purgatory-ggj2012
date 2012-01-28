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
            this.PurgatoryMusic = AudioManager.Instance.CrossFade(this.BackgroundMusic, this.PurgatoryMusic, 1.5f, false);
        }

        public override void PlayerEffect(Player player)
        {
            player.Level = this.PlayerLevel;
            PlayerLevel.PlayPurgatoryAnimation();
            player.Spawn();
            this.BackgroundMusic = AudioManager.Instance.CrossFade(this.PurgatoryMusic, this.BackgroundMusic, 1.5f, true);
        }
    }
}
