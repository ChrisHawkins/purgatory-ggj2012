
namespace Purgatory.Game.Physics
{
    using Microsoft.Xna.Framework;

    public interface IStatic
    {
        Point Position { get; }
        Rectangle CollisionRectangle { get; }
    }
}
