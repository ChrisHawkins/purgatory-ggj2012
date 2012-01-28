
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class LevelGenerator
    {
        public static Level GenerateLevel(string levelType)
        {
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>(levelType + "Maze00");
            bool[,] generatedLevel = new bool[levelTexture.Width, levelTexture.Height];

            Color[] pixelData = new Color[levelTexture.Width * levelTexture.Height];
            levelTexture.GetData<Color>(pixelData);

            for (int i = 0; i < levelTexture.Height; ++i)
            {
                for (int j = 0; j < levelTexture.Width; ++j)
                {
                    Color pixelColor = pixelData[i * levelTexture.Width + j];
                    if (pixelColor == Color.White)
                    {
                        generatedLevel[j, i] = true;
                    }
                    else
                    {
                        generatedLevel[j, i] = false;
                    }
                }
            }

            Level level = new Level(levelType);
            level.LoadLevelData(generatedLevel, levelTexture.Width, levelTexture.Height);

            return level;
        }
    }
}
