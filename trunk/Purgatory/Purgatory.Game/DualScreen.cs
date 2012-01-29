
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.UI;

    public class DualScreen : Screen
    {
        private List<ScreenToDraw> screens;
        private bool escapeWasPressed;
        private Form menu;
        public Cue BackgroundMusic;
        public Cue PurgatoryChaseMusic;
        public bool ChaseMusicPlaying = false;

        public DualScreen(GraphicsDevice device)
            : base(device)
        {
            this.screens = new List<ScreenToDraw>();
            
            this.menu = new Form(device);
            Texture2D texture = BigEvilStatic.Content.Load<Texture2D>("PausedBox");
            SpriteControl pausedBox = new SpriteControl(new Sprite(texture, texture.Width, texture.Height), new Vector2(device.Viewport.Width / 2, device.Viewport.Height / 2));
            menu.Controls.Add(pausedBox);
            
            this.BackgroundMusic = AudioManager.Instance.LoadCue("Purgatory_Gameplay_Joined");
            this.PurgatoryChaseMusic = AudioManager.Instance.LoadCue("Purgatory_PurgatoryChase");
            this.menu.Visible = false;
            Texture2D seperatorTex = BigEvilStatic.Content.Load<Texture2D>("hud");
            Sprite seperator = new Sprite(seperatorTex, seperatorTex.Width, seperatorTex.Height);
            Hud.Controls.Add(new SpriteControl(seperator, new Vector2(device.Viewport.Width / 2, device.Viewport.Height / 2)));
        }

        public override void Draw(Bounds bounds)
        {
            foreach (var toDraw in screens)
            {
                toDraw.Screen.Draw(toDraw.Bounds);
            }

            this.Hud.Draw();

            foreach (var toDraw in screens)
            {
                toDraw.Screen.Hud.Draw();
            }

            this.menu.Draw();
        }

        public void AddScreen(Screen screen)
        {
            this.screens.Add(new ScreenToDraw(screen));

            foreach (var screenInList in this.screens)
            {
                screenInList.Bounds = this.GetBounds(screenInList.Screen);
            }
        }

        private Bounds GetBounds(Screen screen)
        {
            if (this.screens.Count > 2)
            {
                throw new InvalidOperationException("Max 2 screens");
            }

            // VERTICALLY

            //int heightAlloc = Device.Viewport.Height / 2;

            //int i;

            //for (i = 0; i < this.screens.Count; i++)
            //{
            //    if (this.screens[i].Screen == screen) break;
            //}

            //return new Bounds(new Rectangle(0, heightAlloc * i, this.Device.Viewport.Width, heightAlloc));

            // HORIZONTALLY

            int widthAlloc = Device.Viewport.Width / 2;

            int i;

            for (i = 0; i < this.screens.Count; i++)
            {
                if (this.screens[i].Screen == screen) break;
            }

            return new Bounds(new Rectangle(i * widthAlloc, 0, widthAlloc, this.Device.Viewport.Height));
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (this.escapeWasPressed && !kb.IsKeyDown(Keys.Escape))
            {
                if (this.menu.Visible)
                {
                    this.menu.Visible = false;
                    AudioManager.Instance.FadeVolumeUp(1f, 0.25f, 1f);
                }
                else
                {
                    this.menu.Visible = true;
                    AudioManager.Instance.FadeVolumeDown(1f, 0.25f, 1f);
                }
            }

            this.escapeWasPressed = kb.IsKeyDown(Keys.Escape);

            if (this.menu.Visible)
            {
                return;
            }

            this.ContextUpdater(gameTime);

            foreach (var screen in this.screens)
            {
                screen.Screen.Update(gameTime);
            }

            this.Hud.Update(gameTime);
        }

        public Action<GameTime> ContextUpdater { get; set; }

        private class ScreenToDraw
        {
            public Screen Screen;
            public Bounds Bounds;

            public ScreenToDraw(Screen screen)
            {
                Screen = screen;
                Bounds = Bounds.Screen;
            }
        }

        public override void OnControlLost()
        {
            base.OnControlLost();
        }
    }
}
