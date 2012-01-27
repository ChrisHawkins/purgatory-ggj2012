using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Purgatory.Game.Controls
{
    public class KeyboardManager
    {
        KeyboardState keyboard;
        
        private Keys up;
        private Keys down;
        private Keys left;
        private Keys right;
        private Keys shoot;

        public KeyboardManager(Keys up, Keys down, Keys left, Keys right, Keys shoot)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.shoot = shoot;
            this.keyboard = Keyboard.GetState();
        }

        public void Update()
        {
            this.keyboard = Keyboard.GetState();
        }

        public bool UpControlPressed()
        {
            return this.keyboard.IsKeyDown(this.up);
        }

        public bool DownControlPressed()
        {
            return this.keyboard.IsKeyDown(this.down);
        }

        public bool RightControlPressed()
        {
            return this.keyboard.IsKeyDown(this.right);
        }

        public bool LeftControlPressed()
        {
            return this.keyboard.IsKeyDown(this.left);
        }

        public bool ShootControlPressed()
        {
            return this.keyboard.IsKeyDown(this.shoot);
        }

    }
}
