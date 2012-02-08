
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
                Vector2 playerVelocity = 
                    player.Position == player.LastPosition ? 
                    Vector2.Zero : Vector2.Normalize(player.Position - player.LastPosition) * Player.MaxSpeed;
                Vector2 bulletVelocity = Vector2.Normalize(player.BulletDirection) * Player.BulletSpeed;
                Vector2 finalVelocity = playerVelocity + bulletVelocity;

                Bullet b = new Bullet(bulletPos, Vector2.Normalize(finalVelocity), player.BulletBounce, finalVelocity.Length(), new Sprite(player.BulletSprite), player.Level, player.NoClipTime < NoClipPowerUp.Duration, (float)player.NoClipTime.TotalSeconds);
                b.Sprite.Effects.Add(new SpinEffect(200));
                b.Sprite.Embellishments.Add(Embellishment.MakeGlow(player.BulletSpriteName, (float)player.BulletBounce / (Player.MaxBounce / 2.0f), false));
                player.BulletList.Add(b);
                player.Energy -= Player.EnergyPerShot;
                player.ShootTimer = 0.0f;
                AudioManager.Instance.PlayCue(ref player.ShootSFX, true);
            }
        }

        protected abstract bool FirePressed();
        protected abstract Vector2 GetMovementDirection();
        protected abstract bool DashPressed();
    }
}
