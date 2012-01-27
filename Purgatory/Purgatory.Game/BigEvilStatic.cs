
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public static class BigEvilStatic
    {
        public static ContentManager Content { get; private set; }

        public static Viewport Viewport { get; private set; }

        public static void Init(ContentManager manager, Viewport viewport)
        {
            Content = manager;
            Viewport = viewport;
        }
    }
}
