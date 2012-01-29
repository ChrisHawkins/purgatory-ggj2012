
namespace Purgatory.Game.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class KeyboardInputController : InputController
    {
        private KeyboardManager controls;

        public KeyboardInputController(Keys up, Keys down, Keys left, Keys right, Keys shoot, Keys dash)
        {
            this.controls = new KeyboardManager(up, down, left, right, shoot, dash);
        }

        protected override bool FirePressed()
        {
            this.controls.Update();
            return this.controls.ShootControlPressed();
        }

        protected override Vector2 GetMovementDirection()
        {
            this.controls.Update();

            Vector2 movementDirection = new Vector2();

            if (controls.UpControlPressed())
            {
                movementDirection -= Vector2.UnitY;
            }

            if (controls.DownControlPressed())
            {
                movementDirection += Vector2.UnitY;
            }

            if (controls.LeftControlPressed())
            {
                movementDirection -= Vector2.UnitX;
            }

            if (controls.RightControlPressed())
            {
                movementDirection += Vector2.UnitX;
            }

            return movementDirection;
        }

        protected override bool DashPressed()
        {
            this.controls.Update();
            return controls.DashControlPressed();
        }
    }
}
