
namespace Purgatory.Game.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class XboxInputController : InputController
    {
        private PlayerIndex playerIndex;

        public XboxInputController(PlayerIndex player)
        {
            this.playerIndex = player;
        }

        protected override bool FirePressed()
        {
            GamePadState state = GamePad.GetState(this.playerIndex);
            return state.Triggers.Right > 0.5f;
        }

        protected override Vector2 GetMovementDirection()
        {
            GamePadState state = GamePad.GetState(this.playerIndex);
            return state.ThumbSticks.Left * new Vector2(1f, -1f);
        }

        protected override bool DashPressed()
        {
            GamePadState state = GamePad.GetState(this.playerIndex);
            return state.IsButtonDown(Buttons.A);
        }
    }
}
