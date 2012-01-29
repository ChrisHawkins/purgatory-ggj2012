
namespace Purgatory.Game.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public static class NumberFont
    {
        public const float Kerning = -9f;

        private static Texture2D[] font = new Texture2D[10];

        public static void LoadFont(string fontName)
        {
            for (int i = 0; i < font.Length; i++)
            {
                font[i] = BigEvilStatic.Content.Load<Texture2D>(fontName + i);
            }
        }

        public static void DrawNumber(SpriteBatch batch, Vector2 position, int number)
        {
            if (number < 0f) return;

            int[] digits = ConvertToArrayOfDigits(number);
            Texture2D lastTexture = font[0];

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = digits[i];

                var texture = font[digit];
                batch.Draw(texture, position + i * new Vector2(lastTexture.Width + Kerning, 0f), Color.White);
                lastTexture = texture;
            }
        }

        public static float CalculateWidth(int number)
        {
            if (number < 0f) return 0;

            int[] digits = ConvertToArrayOfDigits(number);
            Texture2D lastTexture = font[0];
            float widthAccumulator = 0.0f;

            for (int i = 0; i < digits.Length; i++)
            {
                int digit = digits[i];

                var texture = font[digit];
                widthAccumulator += texture.Width;
            }

            return widthAccumulator;
        }

        private static int[] ConvertToArrayOfDigits(int number)
        {
            int size = number.ToString().Length;
            int[] digits = new int[size];

            for (int index = size - 1; index >= 0; index--)
            {
                digits[index] = number % 10;
                number = number / 10;
            }

            return digits;
        }
    }
}
