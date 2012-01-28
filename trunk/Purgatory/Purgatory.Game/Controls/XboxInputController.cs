
namespace Purgatory.Game.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;
    using Purgatory.Game.Graphics;

    public class XboxInputController : IInputController
    {
        private PlayerIndex playerIndex;

        public XboxInputController(PlayerIndex player)
        {
            this.playerIndex = player;
        }

        public void UpdateMovement(Player player, GameTime gameTime)
        {
            GamePadState state = GamePad.GetState(this.playerIndex);

            if (player.DashVelocity == Vector2.Zero)
            {
                player.MovementDirection = new Vector2();
                player.MovementDirection = state.ThumbSticks.Left * new Vector2(1f, -1f);

                if (player.MovementDirection.LengthSquared() != 0)
                {
                    player.MovementDirection.Normalize();

                    if (state.IsButtonDown(Buttons.A) && player.TimeSinceLastDash > Player.DashCooldownTime && !(player.Level is PurgatoryLevel))
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
            GamePadState state = GamePad.GetState(this.playerIndex);

            player.ShootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state.Triggers.Right > 0.5f && player.ShootTimer > player.ShootCooldown && player.Energy > 0)
            {
                Vector2 bulletPos = player.Position;
                Bullet b = new Bullet(bulletPos, Vector2.Normalize(player.BulletDirection), player.BulletBounce, 500.0f, new Sprite(player.BulletSprite), player.Level);
                player.BulletList.Add(b);
                --player.Energy;
                player.ShootTimer = 0.0f;
                AudioManager.Instance.PlayCue(ref player.ShootSFX, true);
            }

            List<Bullet> tmpBulletList = new List<Bullet>();
            foreach (var bullet in player.BulletList)
            {
                bullet.Update(gameTime);

                if (!bullet.RemoveFromList)
                {
                    tmpBulletList.Add(bullet);
                }
            }

            player.BulletList = tmpBulletList;
        }
    }
}
