
namespace Purgatory.Game.Controls
{
    using Microsoft.Xna.Framework.Input;

    public static class XboxUtility
    {
        private static bool wasDownLastTime;
        private static bool startWasDownLastTime;

        public static bool ButtonPressed(bool includeOtherPlayers = false)
        {
            GamePadState gs = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);

            if (!includeOtherPlayers)
            {
                if (wasDownLastTime)
                {
                    return !ButtonDownInState(gs);
                }

                wasDownLastTime = ButtonDownInState(gs);
            }
            else
            {
                GamePadState gs2 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Two);
                GamePadState gs3 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Three);
                GamePadState gs4 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Four);

                if (wasDownLastTime)
                {
                    return
                        !ButtonDownInState(gs) ||
                        !ButtonDownInState(gs2) ||
                        !ButtonDownInState(gs3) ||
                        !ButtonDownInState(gs4);
                }

                wasDownLastTime =
                    ButtonDownInState(gs) ||
                    ButtonDownInState(gs2) ||
                    ButtonDownInState(gs3) ||
                    ButtonDownInState(gs4);
            }

            return false;
        }

        private static bool ButtonDownInState(GamePadState gs)
        {
            return
                gs.IsButtonDown(Buttons.Start) ||
                gs.IsButtonDown(Buttons.A) ||
                gs.IsButtonDown(Buttons.B) ||
                gs.IsButtonDown(Buttons.X) ||
                gs.IsButtonDown(Buttons.Y) ||
                gs.IsButtonDown(Buttons.BigButton);
        }

        public static bool StartPressed()
        {
            GamePadState gs = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            GamePadState gs2 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Two);
            GamePadState gs3 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Three);
            GamePadState gs4 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Four);

            bool isDown =
                gs.IsButtonDown(Buttons.Start) ||
                gs2.IsButtonDown(Buttons.Start) ||
                gs3.IsButtonDown(Buttons.Start) ||
                gs4.IsButtonDown(Buttons.Start);

            if (!isDown && startWasDownLastTime)
            {
                return true;
            }

            startWasDownLastTime = isDown;
            return false;
        }
    }
}
