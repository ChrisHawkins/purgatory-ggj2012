
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Physics;

    public class Level
    {
        public TileType[][] WalkableTile;

        private int HalfTilesWideOnScreen;
        private int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        private List<Rectangle> rectangles;
        private Sprite wall, wallTop;
        private Sprite backgroundGround;
        private List<PlayerPickUp> pickUps;

        public Level(string levelType)
        {
            // Temp list of rectangles to return
            rectangles = new List<Rectangle>();

            // Set Tiles Wide
            HalfTilesWideOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Width / 4 / TileWidth);
            HalfTilesLongOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Height / 2 / TileWidth);

            // Load level texture;
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>(levelType + "Maze00");

            //Debug textures
            Texture2D wallTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "Wall");
            Texture2D wallTopTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallTop");

            wall = new Sprite(wallTex, TileWidth, TileWidth);
            wallTop = new Sprite(wallTopTex, TileWidth, TileWidth);

            Texture2D backgroundTex = BigEvilStatic.Content.Load<Texture2D>("WhiteWall");
            backgroundGround = new Sprite(backgroundTex, backgroundTex.Width, backgroundTex.Height);

            //Get pixel data as array
            Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            levelTexture.GetData<Color>(pixelData);

            //Fill tile array from pixel data
            WalkableTile = new TileType[levelTexture.Width][];
            for (int i = 0; i < levelTexture.Height; ++i)
            {
                WalkableTile[i] = new TileType[levelTexture.Height];
            }

            for (int i = 0; i < levelTexture.Height; ++i)
            {
                for (int j = 0; j < levelTexture.Width; ++j)
                {
                    Color pixelColor = pixelData[i * levelTexture.Width + j];
                    if (pixelColor == Color.Black)
                    {
                        WalkableTile[j][i] = TileType.Wall;
                    }
                    else if (pixelColor == Color.Red)
                    {
                        WalkableTile[j][i] = TileType.WallTop;
                    }
                    else
                    {
                        WalkableTile[j][i] = TileType.Ground;
                    }
                }
            }

            pickUps = new List<PlayerPickUp>();
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
                        if (WalkableTile[i][j] != TileType.Ground)
                        {
                            rectangles.Add(new Rectangle(i * TileWidth - TileWidth / 2, j * TileWidth - TileWidth / 2, TileWidth, TileWidth));
                        }
                    }
                    else
                    {
                        rectangles.Add(new Rectangle(i * TileWidth - TileWidth / 2, j * TileWidth - TileWidth / 2, TileWidth, TileWidth));
                    }
                }
            }

            return rectangles;
        }

        public void CheckPickUpCollisions(Player player)
        {
            List<PlayerPickUp> tmpList = new List<PlayerPickUp>();

            foreach (var pickUp in this.pickUps)
            {
                if (pickUp.CollisionRectangle.Intersects(player.CollisionRectangle))
                {
                    pickUp.PlayerEffect(player);
                }
                else
                {
                    tmpList.Add(pickUp);
                }
            }

            this.pickUps = tmpList;
        }

        public void AddToPickups(PlayerPickUp pickUp)
        {
            this.pickUps.Add(pickUp);
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

            int left = -(int)bounds.Camera.X - bounds.Rectangle.Width / 2;
            left = left - left.Mod(TileWidth);
            int right = -(int)bounds.Camera.X + bounds.Rectangle.Width / 2;
            right = right + right.Mod(TileWidth);
            int top = -(int)bounds.Camera.Y - bounds.Rectangle.Height / 2;
            top = top - top.Mod(TileWidth);
            int bottom = -(int)bounds.Camera.Y + bounds.Rectangle.Height / 2;
            bottom = bottom + bottom.Mod(TileWidth);

            for (int i = left; i <= right; i += backgroundGround.Width)
            {
                for (int j = top; j <= bottom; j += backgroundGround.Height)
                {
                    backgroundGround.Draw(batch, bounds.AdjustPoint(new Vector2(i, j)));
                }
            }

            for (int i = 0; i < WalkableTile.Length; ++i)
            {
                for (int j = 0; j < WalkableTile[i].Length; ++j)
                {
                    if (WalkableTile[i][j] == TileType.Wall)
                    {
                        this.wall.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                    }
                    else if (WalkableTile[i][j] == TileType.WallTop)
                    {
                        this.wallTop.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                    }
                }
            }

            foreach (var pickup in pickUps)
            {
                pickup.Draw(batch, bounds);
            }
        }
    }
}
