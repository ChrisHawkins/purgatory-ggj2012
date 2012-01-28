
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Microsoft.Xna.Framework.Audio;
    using Purgatory.Game.PowerUps;

    public class Level
    {
        private static readonly Random rng = new Random();
        public TileType[][] WalkableTile;

        protected int HalfTilesWideOnScreen;
        protected int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        protected List<Rectangle> rectangles;
        protected Sprite wall, wallTop, wallBottom, wallLeft, wallRight, wallOutsideCorner, wallTopLeft, wallTopRight, wallBottomLeft, wallBottomRight;
        protected Sprite backgroundGround;
        protected List<PlayerPickUp> pickUps;
        private Cue pickupSFX;
        private const int MaxPickups = 30;

        protected Level() { }

        public Level(string levelType)
        {
            this.pickupSFX = AudioManager.Instance.LoadCue("Purgatory_PickupItem");
            levelType = "life";
            // Temp list of rectangles to return
            rectangles = new List<Rectangle>();

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
            Texture2D wallOutsideTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallOutsideCorner");
            Texture2D wallTopLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallTopLeft");
            Texture2D wallTopRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallTopRight");
            Texture2D wallBottomLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallBottomLeft");
            Texture2D wallBottomRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallBottomRight");


            wall = new Sprite(wallTex, TileWidth, TileWidth);
            wallTop = new Sprite(wallTopTex, TileWidth, TileWidth);
            wallBottom = new Sprite(wallBottomTex, TileWidth, TileWidth);
            wallLeft = new Sprite(wallLeftTex, TileWidth, TileWidth);
            wallRight = new Sprite(wallRightTex, TileWidth, TileWidth);
            wallOutsideCorner = new Sprite(wallOutsideTex, TileWidth, TileWidth);
            wallTopLeft = new Sprite(wallTopLeftTex, TileWidth, TileWidth);
            wallBottomRight = new Sprite(wallBottomRightTex, TileWidth, TileWidth);
            wallBottomLeft = new Sprite(wallBottomLeftTex, TileWidth, TileWidth);
            wallTopRight = new Sprite(wallTopRightTex, TileWidth, TileWidth);

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

        public bool ItemAtLocation(Vector2 location)
        {
            foreach (PlayerPickUp item in pickUps)
            {
                if (item.Position == location)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector2 FindSpawnPoint(bool playerSafe)
        {
            int maxX = this.WalkableTile.Length;
            int maxY = this.WalkableTile[0].Length;
            
            while (true)
            {
                int locX = rng.Next(maxX);
                int locY = rng.Next(maxY);

                if (WalkableTile[locX][locY] == TileType.Ground)
                {
                    if (playerSafe)
                    {
                        // top left
                        if (WalkableTile[locX - 1][locY] == TileType.Ground &&
                            WalkableTile[locX - 1][locY + 1] == TileType.Ground &&
                            WalkableTile[locX][locY + 1] == TileType.Ground)
                        {
                            return new Vector2((locX + locX - 1) / 2.0f, (locY + locY + 1) / 2.0f) * Level.TileWidth;
                        }
                        // top right
                        else if (WalkableTile[locX + 1][locY] == TileType.Ground &&
                                 WalkableTile[locX + 1][locY + 1] == TileType.Ground &&
                                 WalkableTile[locX][locY + 1] == TileType.Ground)
                        {
                            return new Vector2((locX + locX + 1) / 2.0f, (locY + locY + 1) / 2.0f) * Level.TileWidth;
                        }
                        // bottom left
                        else if (WalkableTile[locX - 1][locY] == TileType.Ground &&
                                 WalkableTile[locX - 1][locY - 1] == TileType.Ground &&
                                 WalkableTile[locX][locY - 1] == TileType.Ground)
                        {
                            return new Vector2((locX + locX - 1) / 2.0f, (locY + locY - 1) / 2.0f) * Level.TileWidth;
                        }
                        // bottom left
                        else if (WalkableTile[locX + 1][locY] == TileType.Ground &&
                                 WalkableTile[locX + 1][locY - 1] == TileType.Ground &&
                                 WalkableTile[locX][locY - 1] == TileType.Ground)
                        {
                            return new Vector2((locX + locX + 1) / 2.0f, (locY + locY - 1) / 2.0f) * Level.TileWidth;
                        }
                    }

                    Vector2 loc = new Vector2(locX, locY) * Level.TileWidth;

                    if (!ItemAtLocation(loc))
                    {
                        return loc;
                    }
                }
            }
        }

        public void LoadLevelData(bool[,] data, int width, int height)
        {
            //Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>("DeathMaze00");

            //Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            //levelTexture.GetData<Color>(pixelData);

            this.WalkableTile = new TileType[width][];
            for (int i = 0; i < width; ++i)
            {
                this.WalkableTile[i] = new TileType[height];
            }

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (!data[i, j])
                    {
                        WalkableTile[i][j] = TileType.Wall;

                        if (j + 1 < height && data[i, j + 1])
                        {
                            WalkableTile[i][j] = TileType.WallTop;
                        }
                        else if (j - 1 >= 0 && data[i, j - 1])
                        {
                            WalkableTile[i][j] = TileType.WallBottom;
                        }

                        if (i + 1 < width && data[i + 1, j])
                        {
                            if (WalkableTile[i][j] == TileType.WallTop)
                            {
                                WalkableTile[i][j] = TileType.WallOutsideCorner;
                            }
                            else if (WalkableTile[i][j] == TileType.WallBottom)
                            {
                                WalkableTile[i][j] = TileType.WallOutsideCorner;
                            }
                            else
                            {
                                WalkableTile[i][j] = TileType.WallLeft;
                            }
                        }
                        else if (i - 1 >= 0 && data[i - 1, j])
                        {
                            if (WalkableTile[i][j] == TileType.WallTop)
                            {
                                WalkableTile[i][j] = TileType.WallOutsideCorner;
                            }
                            else if (WalkableTile[i][j] == TileType.WallBottom)
                            {
                                WalkableTile[i][j] = TileType.WallOutsideCorner;
                            }
                            else
                            {
                                WalkableTile[i][j] = TileType.WallRight;
                            }
                        }

                        if (WalkableTile[i][j] == TileType.Wall)
                        {
                            if (i + 1 < width)
                            {
                                if (j - 1 >= 0 && data[i + 1, j - 1])
                                {
                                    WalkableTile[i][j] = TileType.WallBottomLeft;
                                }
                                else if (j + 1 < height && data[i + 1, j + 1])
                                {
                                    WalkableTile[i][j] = TileType.WallTopLeft;
                                }
                            }
                            if (i - 1 >= 0)
                            {
                                if (j - 1 >= 0 && data[i - 1, j - 1])
                                {
                                    WalkableTile[i][j] = TileType.WallBottomRight;
                                }
                                else if (j + 1 < height && data[i - 1, j + 1])
                                {
                                    WalkableTile[i][j] = TileType.WallTopRight;
                                }
                            }
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

            for (int i = minX; i <= maxX; ++i)
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
                    AudioManager.Instance.PlayCue(ref this.pickupSFX, true);
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
            if (this.pickUps.Count < Level.MaxPickups)
            {
                Vector2 loc = FindSpawnPoint(false);
                this.pickUps.Add(pickUp);
                pickUp.SetPosition(loc);
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
                        case TileType.WallOutsideCorner:
                            this.wallOutsideCorner.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallTopLeft:
                            this.wallTopLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallTopRight:
                            this.wallTopRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallBottomLeft:
                            this.wallBottomLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                            break;
                        case TileType.WallBottomRight:
                            this.wallBottomRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
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
