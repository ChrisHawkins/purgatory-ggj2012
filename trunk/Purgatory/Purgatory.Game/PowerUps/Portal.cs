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
        public GameContext gameContext;
        public event EventHandler EscapedPurgatory;

        public Portal(GameContext gameContext)
            : base("Portal")
        {
            this.gameContext = gameContext;
            this.Sprite.Effects.Add(new SpinEffect(1000f));
            this.Sprite.Effects.Add(new GrowShrinkEffect(1000f, 0.2f));
        }

        public override void PlayerEffect(Player player)
        {
            player.Level = this.gameContext.GetNormalLevel(player.PlayerNumber);
            player.Level.PlayPurgatoryAnimation();
            player.Spawn();
            this.EscapedPurgatory(this, EventArgs.Empty);
            this.Sprite.Effects.Clear();
            this.Sprite.Effects.Add(new TemporarySpinEffect(1000f, this.Sprite.Rotation, 5));
            this.Sprite.Effects.Add(new PopInEffect(500, 0.5f, true));
        }
    }
}
