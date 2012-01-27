
namespace Purgatory.Game.Physics
{
    using Microsoft.Xna.Framework;

    public interface IStatic
    {
        Vector2 Position { get; }
        Rectangle CollisionRectangle { get; }
    }
}
