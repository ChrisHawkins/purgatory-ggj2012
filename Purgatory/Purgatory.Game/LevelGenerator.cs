
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System;

    public static class LevelGenerator
    {
        private static readonly Random Rng = new Random();
        public static Level GenerateLevelFromTexture(string levelType, int mapNo, int pairPart)
        {
            Texture2D levelTexture = BigEvilStatic.Content.Load<Texture2D>("Maze" + pairPart + mapNo);
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

        public static Level GenerateLevelRandomly(string levelType)
        {
            int width = 39;
            int height = 39;
            bool[,] generatedLevel = new bool[width * 3, height * 3];
            Stack<Tuple<int, int>> nodes = new Stack<Tuple<int, int>>();
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>();
            List<Tuple<int, int>> otherList = new List<Tuple<int, int>>();

            int currentX = 1;
            int currentY = 1;

            do
            {
                neighbours.Clear();

                for (int i = -2; i <= 2; i += 2)
                {
                    for (int j = -2; j <= 2; j += 2)
                    {
                        if (i != j && i + j != 0)
                        {
                            if (currentX + i >= 0 && currentX + i < width && currentY + j >= 0 && currentY + j < height)
                            {
                                if (!generatedLevel[currentX + i, currentY + j])
                                {
                                    neighbours.Add(new Tuple<int, int>(currentX + i, currentY + j));
                                }
                            }
                        }
                    }
                }

                if (neighbours.Count > 0)
                {
                    otherList.Clear();

                    for (int i = neighbours.Count - 1; i >= 0; --i)
                    {
                        int index = Rng.Next(neighbours.Count);
                        otherList.Add(neighbours[index]);
                        neighbours.RemoveAt(index);
                    }

                    var temp = otherList[0];
                    otherList.RemoveAt(0);

                    if (currentX == temp.Item1)
                    {
                        generatedLevel[currentX, (currentY + temp.Item2) / 2] = true;
                    }
                    else
                    {
                        generatedLevel[(currentX + temp.Item1) / 2, currentY] = true;
                    }

                    currentX = temp.Item1;
                    currentY = temp.Item2;

                    nodes.Push(temp);

                    generatedLevel[currentX, currentY] = true;
                }
                else if (nodes.Count > 0)
                {
                    var temp = nodes.Pop();

                    currentX = temp.Item1;
                    currentY = temp.Item2;
                }
            }
            while (nodes.Count > 0);

            int roomSize = 2;
            for (int i = 0; i < 30; ++i)
            {
                int xIndex = Rng.Next(width);
                int yIndex = Rng.Next(height);

                for (int x = xIndex - roomSize / 2; x <= xIndex + roomSize / 2; ++x)
                {
                    for (int y = yIndex - roomSize / 2; y <= yIndex + roomSize / 2; ++y)
                    {
                        if (x >= 0 && x < width * 3 && y >= 0 && y < height * 3)
                        {
                            generatedLevel[x, y] = true;
                        }
                    }
                }
            }

            for (int i = 0; i < 20; ++i)
            {
                int xIndex = Rng.Next(width);
                int yIndex = Rng.Next(height);

                for (int x = xIndex - roomSize / 2; x <= xIndex + roomSize / 2; ++x)
                {
                    for (int y = yIndex - roomSize / 2; y <= yIndex + roomSize / 2; ++y)
                    {
                        if (x >= 0 && x < width * 3 && y >= 0 && y < height * 3)
                        {
                            generatedLevel[x, y] = false;
                        }
                    }
                }
            }

            int bufferWidth = 10;
            bool[,] finalLevelBools = new bool[width * 3 + bufferWidth * 2, height * 3 + bufferWidth * 2];

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    for (int x = bufferWidth + i * 3; x < i * 3 + 3 + bufferWidth; ++x)
                    {
                        for (int y = j * 3 + bufferWidth; y < j * 3 + 3 + bufferWidth; ++y)
                        {
                            finalLevelBools[x, y] = generatedLevel[i, j];
                        }
                    }
                }
            }

            Level level = new Level(levelType);
            level.LoadLevelData(finalLevelBools, width * 3, height * 3);

            return level;
        }
    }
}
