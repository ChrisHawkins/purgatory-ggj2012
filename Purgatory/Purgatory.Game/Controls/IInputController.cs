
namespace Purgatory.Game.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public interface IInputController
    {
        void UpdateMovement(Player player, GameTime gameTime);
        void UpdateShoot(Player player, GameTime gameTime);
    }
}
