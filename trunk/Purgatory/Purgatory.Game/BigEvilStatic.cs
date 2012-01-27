
namespace Purgatory.Game
{
    using Microsoft.Xna.Framework.Content;

    public static class BigEvilStatic
    {
        public static ContentManager Content { get; private set; }

        public static void Init(ContentManager manager)
        {
            Content = manager;
        }
    }
}
