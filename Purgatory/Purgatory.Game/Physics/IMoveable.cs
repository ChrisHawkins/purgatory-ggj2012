
namespace Purgatory.Game.Physics
{
    using Microsoft.Xna.Framework;

    public interface IMoveable : IStatic
    {
        Point Position { get; set; }
    }
}
