
namespace Purgatory.Game.Controls
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Graphics;

    public class KeyboardInputController : IInputController
    {
        private KeyboardManager controls;

        

        public KeyboardInputController(Keys up, Keys down, Keys left, Keys right, Keys shoot, Keys dash)
        {
            this.controls = new KeyboardManager(up, down, left, right, shoot, dash);
        }

        public void UpdateMovement(Player player, GameTime gameTime)
        {
            this.controls.Update();

            if (player.DashVelocity == Vector2.Zero)
            {
                player.MovementDirection = new Vector2();

                if (controls.UpControlPressed())
                {
                    player.MovementDirection -= Vector2.UnitY;
                }

                if (controls.DownControlPressed())
                {
                    player.MovementDirection += Vector2.UnitY;
                }

                if (controls.LeftControlPressed())
                {
                    player.MovementDirection -= Vector2.UnitX;
                }

                if (controls.RightControlPressed())
                {
                    player.MovementDirection += Vector2.UnitX;
                }

                if (player.MovementDirection.LengthSquared() != 0)
                {
                    player.MovementDirection = Vector2.Normalize(player.MovementDirection);

                    if (controls.DashControlPressed() && player.TimeSinceLastDash > Player.DashCooldownTime)
                    {
                        AudioManager.Instance.PlayCue(ref player.DashSFX, false);
                        player.DashVelocity = player.Speed * 5 * player.MovementDirection;
                        player.TimeSinceLastDash = 0;
                    }
                }
            }
        }

        public void UpdateShoot(Player player, GameTime time)
        {
            this.controls.Update();

            player.ShootTimer += (float)time.ElapsedGameTime.TotalSeconds;
            if (this.controls.ShootControlPressed() && player.ShootTimer > player.ShootCooldown && player.Energy > 0)
            {
                Vector2 bulletPos = player.Position;
                Bullet b = new Bullet(bulletPos, Vector2.Normalize(player.BulletDirection), player.BulletBounce, 500.0f, new Sprite(player.BulletSprite), player.Level);
                player.BulletList.Add(b);
                --player.Energy;
                player.ShootTimer = 0.0f;
                AudioManager.Instance.PlayCue(ref player.ShootSFX, true);
            }
        }
    }
}
