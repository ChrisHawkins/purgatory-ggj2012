
namespace Purgatory.Game.PowerUps
{
    using System;

    public class NoClipPowerUp : PlayerPickUp
    {
        public static readonly TimeSpan Duration = TimeSpan.FromSeconds(10);

        public NoClipPowerUp() : base("NoClipPowerUp")
        {
        }

        public override void PlayerEffect(Player player)
        {
            player.NoClipTime = Duration;
        }
    }
}
