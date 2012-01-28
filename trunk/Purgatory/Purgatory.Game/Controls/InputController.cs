
namespace Purgatory.Game.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Animation;
    using Purgatory.Game.PowerUps;

    public abstract class InputController
    {
        public void UpdateMovement(Player player, GameTime gameTime)
        {
            if (player.DashVelocity == Vector2.Zero)
            {
                player.MovementDirection = this.GetMovementDirection();

                if (player.MovementDirection.LengthSquared() != 0)
                {
                    player.MovementDirection = Vector2.Normalize(player.MovementDirection);

                    if (this.DashPressed() && player.TimeSinceLastDash > Player.DashCooldownTime)
                    {
                        AudioManager.Instance.PlayCue(ref player.DashSFX, false);
                        player.DashVelocity = player.Speed * 5 * player.MovementDirection;
                        player.TimeSinceLastDash = 0;
                    }
                }
            }
        }

        public void UpdateShoot(Player player, GameTime gameTime)
        {
            player.ShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.FirePressed() && player.ShootTimer > player.ShootCooldown && player.Energy > 0)
            {
                Vector2 bulletPos = player.Position;

                Bullet b = new Bullet(bulletPos, Vector2.Normalize(player.BulletDirection), player.BulletBounce, 500.0f, new Sprite(player.BulletSprite), player.Level, player.NoClipTime < NoClipPowerUp.Duration);
                b.Sprite.Effects.Add(new SpinEffect(200));
                player.BulletList.Add(b);
                --player.Energy;
                player.ShootTimer = 0.0f;
                AudioManager.Instance.PlayCue(ref player.ShootSFX, true);
            }
        }

        protected abstract bool FirePressed();
        protected abstract Vector2 GetMovementDirection();
        protected abstract bool DashPressed();
    }
}
