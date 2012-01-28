
namespace Purgatory.Game.Physics
{
    using Microsoft.Xna.Framework;

    public interface IMoveable : IStatic
    {
        Vector2 Position { get; }
        Vector2 LastPosition { get; }
    }
}
