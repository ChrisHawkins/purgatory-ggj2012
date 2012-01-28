
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    public static class LevelGenerator
    {
        public static Level GenerateLevel(string levelType)
        {
            bool[,] generatedLevel = new bool[100, 100];



            Level level = new Level(levelType);
            level.LoadLevelData(generatedLevel);

            return level;
        }
    }
}
