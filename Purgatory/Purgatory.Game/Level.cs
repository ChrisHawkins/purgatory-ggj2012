
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Level
    {
        public bool[][] WalkableTile;

        private int TilesWideOnScreen;
        private int TilesLongOnScreen;

        public const int TileWidth = 32;

        public Level(string levelTextureString)
        {
            // Load level texture;
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>(levelTextureString);

            //Get pixel data as array
            Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            levelTexture.GetData<Color>(pixelData);

            //Fill tile array from pixel data
            WalkableTile = new bool[levelTexture.Width][];
            for (int i = 0; i < levelTexture.Width; ++i )
            {
                WalkableTile[i] = new bool[levelTexture.Height];
                for (int j = 0; j < levelTexture.Height; ++j)
                {
                    if (pixelData[i * levelTexture.Width + j] != Color.Black)
                    {
                        WalkableTile[i][j] = true;
                    }
                }
            }
        }

        public void Draw(Point playerPosition)
        {
            for (int i = playerPosition.X; i < playerPosition.X + TilesWideOnScreen / 2; ++i)
            {
                for (int j = playerPosition.Y; j < playerPosition.Y + TilesLongOnScreen / 2; ++j)
                {

                }
            }
        }
    }
}
