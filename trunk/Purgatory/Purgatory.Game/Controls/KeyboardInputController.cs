using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Purgatory.Game.Graphics;

namespace Purgatory.Game.Controls
{
    public class KeyboardInputController : IInputController
    {
        private KeyboardManager controls;

        private float shootCooldown;
        private float shootTimer;

        public KeyboardInputController(Keys up, Keys down, Keys left, Keys right, Keys shoot, Keys dash)
        {
            this.controls = new KeyboardManager(up, down, left, right, shoot, dash);
            this.shootCooldown = 0.2f;
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
                    player.MovementDirection.Normalize();

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

            this.shootTimer += (float)time.ElapsedGameTime.TotalSeconds;
            if (this.controls.ShootControlPressed() && this.shootTimer > this.shootCooldown && player.Energy > 0)
            {
                Vector2 bulletPos = player.Position;
                Bullet b = new Bullet(bulletPos, player.BulletDirection, player.BulletBounce, 500.0f, new Sprite(player.BulletSprite), player.Level);
                player.BulletList.Add(b);
                --player.Energy;
                this.shootTimer = 0.0f;
                AudioManager.Instance.PlayCue(ref player.ShootSFX, true);
            }

            List<Bullet> tmpBulletList = new List<Bullet>();
            foreach (var bullet in player.BulletList)
            {
                bullet.Update(time);
                if (!bullet.RemoveFromList)
                {
                    tmpBulletList.Add(bullet);
                }

            }

            player.BulletList = tmpBulletList;
        }
    }
}
