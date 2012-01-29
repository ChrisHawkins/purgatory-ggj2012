
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Graphics;
    using Microsoft.Xna.Framework.Audio;
    using Purgatory.Game.PowerUps;
    using Purgatory.Game.Animation;

    public class Level
    {
        private static readonly Random rng = new Random();
        public TileType[][] WalkableTile;

        private Sprite purgatoryOverlay;

        protected int HalfTilesWideOnScreen;
        protected int HalfTilesLongOnScreen;

        public const int TileWidth = 32;

        protected List<Rectangle> rectangles;
        protected Sprite wall, wallTop, wallBottom, wallLeft, wallRight, wallOutsideTopLeft, wallOutsideTopRight, wallOutsideBottomLeft, wallOutsideBottomRight, wallInsideTopLeft, wallInsideTopRight, wallInsideBottomLeft, wallInsideBottomRight;
        protected Sprite backgroundGround;
        protected List<PlayerPickUp> pickUps;
        protected List<PlayerPickUp> removedPickUps;
        protected Cue pickupSFX;
        private const int MaxPickups = 30;

        private Sprite purgatoryText, findPortalText;

        protected Level()
        {
            this.purgatoryOverlay = new Sprite(BigEvilStatic.Content.Load<Texture2D>("WhiteOut"), 48, 48);
            this.purgatoryOverlay.Zoom = 100f;
            this.purgatoryOverlay.Alpha = 0f;

            this.removedPickUps = new List<PlayerPickUp>();

            Texture2D purgTextTex = BigEvilStatic.Content.Load<Texture2D>("purgatory");
            Texture2D portalTextTex = BigEvilStatic.Content.Load<Texture2D>("findportal");

            purgatoryText = new Sprite(purgTextTex, purgTextTex.Width, purgTextTex.Height);
            findPortalText = new Sprite(portalTextTex, portalTextTex.Width, portalTextTex.Height);
        }

        public Level(string levelType) : this()
        {
            this.pickupSFX = AudioManager.Instance.LoadCue("Purgatory_PickupItem");

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
            Texture2D wallInsideTopLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallInsideTopLeft");
            Texture2D wallInsideTopRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallInsideTopRight");
            Texture2D wallInsideBottomLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallInsideBottomLeft");
            Texture2D wallInsideBottomRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallInsideBottomRight");
            Texture2D wallOutsideTopLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallOutsideTopLeft");
            Texture2D wallOutsideTopRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallOutsideTopRight");
            Texture2D wallOutsideBottomLeftTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallOutsideBottomLeft");
            Texture2D wallOutsideBottomRightTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallOutsideBottomRight");


            wall = new Sprite(wallTex, TileWidth, TileWidth);
            wallTop = new Sprite(wallTopTex, TileWidth, TileWidth);
            wallBottom = new Sprite(wallBottomTex, TileWidth, TileWidth);
            wallLeft = new Sprite(wallLeftTex, TileWidth, TileWidth);
            wallRight = new Sprite(wallRightTex, TileWidth, TileWidth);
            wallInsideTopLeft = new Sprite(wallInsideTopLeftTex, TileWidth, TileWidth);
            wallInsideBottomRight = new Sprite(wallInsideBottomRightTex, TileWidth, TileWidth);
            wallInsideBottomLeft = new Sprite(wallInsideBottomLeftTex, TileWidth, TileWidth);
            wallInsideTopRight = new Sprite(wallInsideTopRightTex, TileWidth, TileWidth);
            wallOutsideTopLeft = new Sprite(wallOutsideTopLeftTex, TileWidth, TileWidth);
            wallOutsideBottomRight = new Sprite(wallOutsideBottomRightTex, TileWidth, TileWidth);
            wallOutsideBottomLeft = new Sprite(wallOutsideBottomLeftTex, TileWidth, TileWidth);
            wallOutsideTopRight = new Sprite(wallOutsideTopRightTex, TileWidth, TileWidth);

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

        internal void PlayPurgatoryAnimation()
        {
            this.purgatoryOverlay.Effects.Add(new PurgatoryEffect());
        }

        public virtual void Update(GameTime gameTime)
        {
            this.purgatoryOverlay.UpdateEffects(gameTime);

            foreach (var item in this.pickUps)
            {
                item.Sprite.UpdateEffects(gameTime);
            }

            List<PlayerPickUp> toRemove = new List<PlayerPickUp>();

            foreach (var item in this.removedPickUps)
            {
                item.Sprite.UpdateEffects(gameTime);

                if (item.Sprite.Effects.Count == 0)
                {
                    toRemove.Add(item);
                }
            }

            foreach (var item in toRemove)
            {
                this.removedPickUps.Remove(item);
            }
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

        public int GetItemCount(Type item)
        {
            int count = 0;
            foreach (var otherItem in this.pickUps)
            {
                if (this.pickUps.GetType() == item)
                {
                    ++count;
                }
            }
            return count;
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
                                WalkableTile[i][j] = TileType.WallOutsideBottomRight;
                            }
                            else if (WalkableTile[i][j] == TileType.WallBottom)
                            {
                                WalkableTile[i][j] = TileType.WallOutsideTopRight;
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
                                WalkableTile[i][j] = TileType.WallOutsideBottomLeft;
                            }
                            else if (WalkableTile[i][j] == TileType.WallBottom)
                            {
                                WalkableTile[i][j] = TileType.WallOutsideTopLeft;
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
                                    WalkableTile[i][j] = TileType.WallInsideBottomLeft;
                                }
                                else if (j + 1 < height && data[i + 1, j + 1])
                                {
                                    WalkableTile[i][j] = TileType.WallInsideTopLeft;
                                }
                            }
                            if (i - 1 >= 0)
                            {
                                if (j - 1 >= 0 && data[i - 1, j - 1])
                                {
                                    WalkableTile[i][j] = TileType.WallInsideBottomRight;
                                }
                                else if (j + 1 < height && data[i - 1, j + 1])
                                {
                                    WalkableTile[i][j] = TileType.WallInsideTopRight;
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
            for (int i = pickUps.Count - 1; i >= 0; --i)
            {
                if (pickUps[i].CollisionRectangle.Intersects(player.CollisionRectangle))
                {
                    pickUps[i].PlayerEffect(player);
                    AudioManager.Instance.PlayCue(ref this.pickupSFX, true);
                    this.removedPickUps.Add(pickUps[i]);
                    pickUps[i].Sprite.Effects.Add(new PopInEffect(150f, 0.2f, true));
                    pickUps.RemoveAt(i);
                }
            }
        }

        public virtual void AddToPickups(PlayerPickUp pickUp, bool safe4)
        {
            if (this.pickUps.Count < Level.MaxPickups)
            {
                Vector2 loc = FindSpawnPoint(safe4);
                this.pickUps.Add(pickUp);
                pickUp.SetPosition(loc);
                AnimatePickUpIn(pickUp);
            }
        }

        private void AnimatePickUpIn(PlayerPickUp pickUp)
        {
            pickUp.Sprite.Zoom = 0.0f;
            pickUp.Sprite.Effects.Add(new PopInEffect(400f, 0.2f));
        }

        public void ClearPickups()
        {
            this.pickUps.Clear();
        }

        public virtual void AddToPickups(PlayerPickUp pickUp, Vector2 playerPosition, int minDistance, bool safe4)
        {
            if (this.pickUps.Count < Level.MaxPickups)
            {
                Vector2 loc;
                do
                {
                    loc = FindSpawnPoint(safe4);
                }
                while(Vector2.DistanceSquared(loc, playerPosition) < minDistance * minDistance);

                this.pickUps.Add(pickUp);
                pickUp.SetPosition(loc);
                this.AnimatePickUpIn(pickUp);
            }
        }
        
        public virtual void Draw(SpriteBatch batch, Bounds bounds)
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

            int numberTilesAcross = bounds.Rectangle.Width / TileWidth + 3;
            int numberTilesUp = bounds.Rectangle.Height / TileWidth + 3;
            int xIndex = (int)-bounds.Camera.X / TileWidth + 1;
            int yIndex = (int)-bounds.Camera.Y / TileWidth + 1;

            for (int i = xIndex - numberTilesAcross / 2; i <= xIndex + numberTilesAcross / 2; ++i)
            {
                if(i >= 0 && i < WalkableTile.Length)
                {
                    for (int j = yIndex - numberTilesUp / 2; j <= yIndex + numberTilesUp / 2; ++j)
                    {
                        if (j >= 0 && j < WalkableTile[i].Length)
                        {
                            switch ((int)WalkableTile[i][j])
                            {
                                case (int)TileType.Wall:
                                    this.wall.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallTop:
                                    this.wallTop.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallBottom:
                                    this.wallBottom.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallLeft:
                                    this.wallLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallRight:
                                    this.wallRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallInsideTopLeft:
                                    this.wallInsideTopLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallInsideTopRight:
                                    this.wallInsideTopRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallInsideBottomLeft:
                                    this.wallInsideBottomLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallInsideBottomRight:
                                    this.wallInsideBottomRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallOutsideTopLeft:
                                    this.wallOutsideTopLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallOutsideTopRight:
                                    this.wallOutsideTopRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallOutsideBottomLeft:
                                    this.wallOutsideBottomLeft.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                                case (int)TileType.WallOutsideBottomRight:
                                    this.wallOutsideBottomRight.Draw(batch, bounds.AdjustPoint(new Vector2(i * TileWidth, j * TileWidth)));
                                    break;
                            }
                        }
                    }
                }
            }

            foreach (var pickup in pickUps)
            {
                pickup.Draw(batch, bounds);
            }

            foreach (var pickup in this.removedPickUps)
            {
                pickup.Draw(batch, bounds);
            }

            this.purgatoryOverlay.Draw(batch, new Vector2(0f, 0f), false);
            if (this.purgatoryOverlay.Alpha > 0)
            {
                findPortalText.Draw(batch, new Vector2(bounds.Rectangle.Width / 2, bounds.Rectangle.Height / 2 + 50));
                purgatoryText.Draw(batch, new Vector2(bounds.Rectangle.Width / 2, bounds.Rectangle.Height / 2 - 50));
            }
        }
    }
}
