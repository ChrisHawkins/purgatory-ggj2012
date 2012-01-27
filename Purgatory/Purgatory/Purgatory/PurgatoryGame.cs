
namespace Purgatory
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Purgatory.Game;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PurgatoryGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private ScreenManager screenManager;

        public PurgatoryGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
            this.graphics.ApplyChanges();

            this.screenManager = new ScreenManager();

            BigEvilStatic.Init(this.screenManager, this.Content, this.GraphicsDevice.Viewport);

            this.screenManager.ScreensEmpty += new EventHandler(ScreenManagerEmpty);
            this.screenManager.OpenScreen(new MainMenu(this.GraphicsDevice));
        }

        private void ScreenManagerEmpty(object sender, EventArgs e)
        {
            this.Exit();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Fuchsia);

            this.screenManager.Draw();

            base.Draw(gameTime);
        }
    }
}
