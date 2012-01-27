
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;

    public class Level
    {
        public bool[][] WalkableTile;

        private int HalfTilesWideOnScreen;
        private int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        private List<Rectangle> rectangles;
        private Sprite whiteWall, blackWall;

        public Level(string levelTextureString)
        {
            // Temp list of rectangles to return
            rectangles = new List<Rectangle>();

            // Set Tiles Wide
            HalfTilesWideOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Width / 4 / TileWidth);
            HalfTilesLongOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Height / 2 / TileWidth);

            // Load level texture;
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>(levelTextureString);

            //Debug textures
            Texture2D whiteWallTex = BigEvilStatic.Content.Load<Texture2D>("WhiteWall");
            Texture2D blackWallTex = BigEvilStatic.Content.Load<Texture2D>("BlackWall");

            whiteWall = new Sprite(whiteWallTex, TileWidth, TileWidth);
            blackWall = new Sprite(blackWallTex, TileWidth, TileWidth);

            //Get pixel data as array
            Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            levelTexture.GetData<Color>(pixelData);

            //Fill tile array from pixel data
            WalkableTile = new bool[levelTexture.Width][];
            for (int i = 0; i < levelTexture.Height; ++i)
            {
                WalkableTile[i] = new bool[levelTexture.Height];
            }

            for (int i = 0; i < levelTexture.Height; ++i)
            {
                for (int j = 0; j < levelTexture.Width; ++j)
                {
                    if (pixelData[i * levelTexture.Width + j] != Color.Black)
                    {
                        WalkableTile[j][i] = true;
                    }
                }
            }
        }

        public List<Rectangle> GetPossibleRectangles(Vector2 position, Vector2 lastPosition)
        {
            rectangles.Clear();

            Vector2 min = Vector2.Min(position, lastPosition);// / TileWidth;
            Vector2 max = Vector2.Max(position, lastPosition);// / TileWidth;

            int minX = (int)Math.Floor((min.X - TileWidth / 2) / TileWidth);
            int minY = (int)Math.Floor((min.Y - TileWidth / 2) / TileWidth);

            int maxX = (int)Math.Ceiling((max.X + TileWidth / 2) / TileWidth);
            int maxY = (int)Math.Ceiling((max.Y + TileWidth / 2) / TileWidth);

            for (int i = minX; i <= maxX; ++i )
            {
                for (int j = minY; j <= maxY; ++j)
                {
                    if (i >= 0 && i < WalkableTile.Length && j >= 0 && j < WalkableTile[i].Length)
                    {
                        if (!WalkableTile[i][j])
                        {
                            rectangles.Add(new Rectangle(i * TileWidth - TileWidth / 2, j * TileWidth - TileWidth / 2, TileWidth, TileWidth));
                        }
                    }
                }
            }

            return rectangles;
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            //for (int i = -HalfTilesWideOnScreen; i < HalfTilesWideOnScreen + 1; ++i)
            //{
            //    for (int j = -HalfTilesLongOnScreen; j < HalfTilesLongOnScreen + 1; ++j)
            //    {
            //        int xTileIndex = (int)bounds.Camera.X + i;
            //        int yTileIndex = (int)bounds.Camera.Y + j;

            //        if (xTileIndex >= 0 && xTileIndex < WalkableTile.Length && yTileIndex >= 0 && yTileIndex < WalkableTile[xTileIndex].Length)
            //        {
            //            if (WalkableTile[xTileIndex][yTileIndex])
            //            {
            //                this.whiteWall.Draw(batch, bounds.AdjustPoint(new Vector2(xTileIndex * TileWidth + TileWidth / 2, yTileIndex * TileWidth + TileWidth / 2)));
            //            }
            //            else
            //            {
            //                this.blackWall.Draw(batch, bounds.AdjustPoint(new Vector2(xTileIndex * TileWidth + TileWidth / 2, yTileIndex * TileWidth + TileWidth / 2)));
            //            }
            //        }
            //        else
            //        {
            //            this.blackWall.Draw(batch, bounds.AdjustPoint(new Vector2(xTileIndex * TileWidth + TileWidth / 2, yTileIndex * TileWidth + TileWidth / 2)));
            //        }
            //    }
            //}

            for (int i = 0; i < WalkableTile.Length; ++i)
            {
                for (int j = 0; j < WalkableTile[i].Length; ++j)
                {
                    if (WalkableTile[i][j])
                    {
                        this.whiteWall.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                    }
                    else
                    {
                        this.blackWall.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                    }
                }
            }
        }
    }
}
