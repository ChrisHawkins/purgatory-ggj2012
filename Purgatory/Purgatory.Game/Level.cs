
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;

    public class Level
    {
        private Random rng;
        public TileType[][] WalkableTile;

        protected int HalfTilesWideOnScreen;
        protected int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        protected List<Rectangle> rectangles;
        protected Sprite wall, wallTop, wallBottom, wallLeft, wallRight;
        protected Sprite backgroundGround;
        protected List<PlayerPickUp> pickUps;

        protected Level() { }

        public Level(string levelType)
        {
            levelType = "life";
            // Temp list of rectangles to return
            rectangles = new List<Rectangle>();
            rng = new Random();

            // Set Tiles Wide
            HalfTilesWideOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Width / 4 / TileWidth);
            HalfTilesLongOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Height / 2 / TileWidth);

            // Load level texture;
            //Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>(levelType + "Maze00");

            //Debug textures
            Texture2D wallTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "Wall");
            Texture2D wallTopTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallTop");
            Texture2D wallBottomTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallBottom");
            Texture2D wallLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallLeft");
            Texture2D wallRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallRight");


            wall = new Sprite(wallTex, TileWidth, TileWidth);
            wallTop = new Sprite(wallTopTex, TileWidth, TileWidth);
            wallBottom = new Sprite(wallBottomTex, TileWidth, TileWidth);
            wallLeft = new Sprite(wallLeftTex, TileWidth, TileWidth);
            wallRight = new Sprite(wallRightTex, TileWidth, TileWidth);

            Texture2D backgroundTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "Ground");
            backgroundGround = new Sprite(backgroundTex, backgroundTex.Width, backgroundTex.Height);

            //for (int i = 0; i < WalkableTile.Length; ++i)
            //{
            //    for (int j = 0; j < WalkableTile[i].Length; ++j)
            //    {
            //        if (WalkableTile[i][j] == TileType.Wall)
            //        {
            //            if (j + 1 < WalkableTile[i].Length && WalkableTile[i][j + 1] == TileType.Ground)
            //            {
            //                WalkableTile[i][j] = TileType.WallTop;
            //            }
            //            else if (j - 1 >= 0 && WalkableTile[i][j - 1] == TileType.Ground)
            //            {
            //                WalkableTile[i][j] = TileType.WallBottom;
            //            }
            //        }
            //    }
            //}

            pickUps = new List<PlayerPickUp>();
        }

        public void LoadLevelData(bool[,] data, int width, int height)
        {
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>("DeathMaze00");

            Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            levelTexture.GetData<Color>(pixelData);

            this.WalkableTile = new TileType[levelTexture.Width][];
            for (int i = 0; i < levelTexture.Height; ++i)
            {
                this.WalkableTile[i] = new TileType[levelTexture.Height];
            }

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (!data[i, j])
                    {
                        if (j + 1 < height && !data[i,j + 1])
                        {
                            WalkableTile[i][j] = TileType.WallTop;
                        }
                        else if (j - 1 >= 0 && !data[i,j - 1])
                        {
                            WalkableTile[i][j] = TileType.WallBottom;
                        }
                        else if (i + 1 < width && !data[i + 1,j])
                        {
                            WalkableTile[i][j] = TileType.WallLeft;
                        }
                        else if (i - 1 >= 0 && data[i - 1, j])
                        {
                            WalkableTile[i][j] = TileType.WallRight;
                        }
                        else
                        {
                            WalkableTile[i][j] = TileType.Wall;
                        }
                    }
                    else
                    {
                        WalkableTile[i][j] = TileType.Ground;
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
            int maxX = this.WalkableTile.Length;
            int maxY = this.WalkableTile[0].Length;

            bool ammoPlaced = false;
            while (!ammoPlaced)
            {
                int locX = rng.Next(maxX);
                int locY = rng.Next(maxY);

                if (WalkableTile[locX][locY] == TileType.Ground)
                {
                    ammoPlaced = true;
                    this.pickUps.Add(pickUp);
                    pickUp.SetPosition(new Vector2(locX, locY) * Level.TileWidth);
                }
            }
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            int numAcross = bounds.Rectangle.Width / backgroundGround.Width + 3;
            int numUp = bounds.Rectangle.Height / backgroundGround.Height + 3;
            int playerBGIndexX = (int)-bounds.Camera.X / backgroundGround.Width + 1;
            int playerBGIndexY = (int)-bounds.Camera.Y / backgroundGround.Height + 1;

            for (int i = playerBGIndexX - numAcross / 2; i <= playerBGIndexX + numAcross / 2; ++i)
            {
                for (int j = playerBGIndexY - numUp / 2; j <= playerBGIndexY + numUp / 2; ++j)
                {
                    backgroundGround.Draw(batch, bounds.AdjustPoint(new Vector2(i * backgroundGround.Width, j * backgroundGround.Height)));
                }
            }

            for (int i = 0; i < WalkableTile.Length; ++i)
            {
                for (int j = 0; j < WalkableTile[i].Length; ++j)
                {
                    switch (WalkableTile[i][j])
                    {
                        case TileType.Wall:
                            this.wall.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallTop:
                            this.wallTop.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallBottom:
                            this.wallBottom.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallLeft:
                            this.wallLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallRight:
                            this.wallRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
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
