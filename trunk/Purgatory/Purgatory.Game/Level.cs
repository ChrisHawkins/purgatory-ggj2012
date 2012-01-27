﻿
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

        private int HalfTilesWideOnScreen;
        private int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        private List<Rectangle> rectangles;

        public Level(string levelTextureString)
        {
            // Temp list of rectangles to return
            rectangles = new List<Rectangle>();

            // Set Tiles Wide
            HalfTilesWideOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Width / 4 / TileWidth);
            HalfTilesLongOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Height / 4 / TileWidth);

            // Load level texture;
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>(levelTextureString);

            //Get pixel data as array
            Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            levelTexture.GetData<Color>(pixelData);

            //Fill tile array from pixel data
            WalkableTile = new bool[levelTexture.Width][];
            for (int i = 0; i < levelTexture.Width; ++i)
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

        public List<Rectangle> GetPossibleRectangles(Vector2 playerPosition, Vector2 velocity)
        {
            rectangles.Clear();

            return rectangles;
        }

        public void Draw(Vector2 playerPosition)
        {
            for (int i = -HalfTilesWideOnScreen; i < HalfTilesWideOnScreen; ++i)
            {
                for (int j = -HalfTilesWideOnScreen; j < HalfTilesLongOnScreen; ++j)
                {
                    int xTileIndex = (int)playerPosition.X + i;
                    int yTileIndex = (int)playerPosition.Y + j;

                    if (xTileIndex >= 0 && xTileIndex < WalkableTile.Length && yTileIndex >= 0 && yTileIndex < WalkableTile[i].Length)
                    {
                        if (WalkableTile[i][j])
                        {
                            //Draw GroundTile
                        }
                        else
                        {
                            //Draw WallTile
                        }
                    }
                }
            }
        }
    }
}
