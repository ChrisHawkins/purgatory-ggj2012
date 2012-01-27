using System;

namespace Purgatory
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Purgatory game = new Purgatory())
            {
                game.Run();
            }
        }
    }
#endif
}

