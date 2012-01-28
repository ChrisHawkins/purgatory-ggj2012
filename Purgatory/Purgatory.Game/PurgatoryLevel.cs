using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Purgatory.Game.Graphics;
using Purgatory.Game.PowerUps;

namespace Purgatory.Game
{
    public class PurgatoryLevel : Level
    {
        public PurgatoryLevel(string levelType, TileType[][] maze1, TileType[][] maze2) : base()
        {
            // Temp list of rectangles to return
            rectangles = new List<Rectangle>();

            // Set Tiles Wide
            HalfTilesWideOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Width / 4 / TileWidth);
            HalfTilesLongOnScreen = (int)Math.Ceiling((double)BigEvilStatic.Viewport.Height / 2 / TileWidth);

            //Debug textures
            Texture2D wallTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "Wall");
            Texture2D wallTopTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "WallTop");

            wall = new Sprite(wallTex, TileWidth, TileWidth);
            wallTop = new Sprite(wallTopTex, TileWidth, TileWidth);

            Texture2D backgroundTex = BigEvilStatic.Content.Load<Texture2D>(levelType + "Ground");
            backgroundGround = new Sprite(backgroundTex, backgroundTex.Width, backgroundTex.Height);

            //Fill tile array from pixel data
            WalkableTile = new TileType[maze1.Length][];
            for (int i = 0; i < maze1.Length; ++i)
            {
                WalkableTile[i] = new TileType[maze1[i].Length];
            }

            for (int i = 0; i < WalkableTile.Length; ++i)
            {
                for (int j = 0; j < WalkableTile[i].Length; ++j)
                {
                    if (maze1[i][j] == TileType.Ground || maze2[i][j] == TileType.Ground)
                    {
                        WalkableTile[i][j] = TileType.Ground;
                    }
                    else
                    {
                        WalkableTile[i][j] = TileType.Wall;
                    }
                }
            }

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
    }
}
