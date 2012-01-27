
namespace Purgatory.Game.Physics
{
    using System;
    using Microsoft.Xna.Framework;

    public class CollisionSolver
    {
        public void SolveCollision(IMoveable moveableObject, IMoveable moveableObject2)
        {
            throw new NotImplementedException("Moveable can't hit moveable");
        }

        public void SolveCollision(IMoveable moveableObject, IStatic staticObject)
        {
            Rectangle rect1 = GeometryUtility.GetAdjustedRectangle(moveableObject.Position, moveableObject.CollisionRectangle);
            Rectangle rect2 = GeometryUtility.GetAdjustedRectangle(staticObject.Position, staticObject.CollisionRectangle);

            if (rect1.Intersects(rect2))
            {
                int differenceX = 0;
                int differenceY = 0;

                if (rect2.Top < rect1.Bottom)
                {
                    differenceY = rect2.Top + (rect1.Bottom - rect2.Top);
                }
                if (rect1.Top < rect2.Bottom)
                {
                    differenceY = rect1.Top + (rect2.Bottom - rect1.Top);
                }

                if (rect2.Left < rect1.Right)
                {
                    differenceX = rect2.Left + (rect1.Right - rect2.Left);
                }
                if (rect1.Left < rect2.Right)
                {
                    differenceX = rect1.Left + (rect2.Right - rect1.Left);
                }

                moveableObject.Position = new Vector2(
                    moveableObject.Position.X + differenceX,
                    moveableObject.Position.Y + differenceY);
            }
        }

        public void SolveCollision(IMoveable moveableObject, Rectangle staticObject)
        {
            Rectangle rect1 = GeometryUtility.GetAdjustedRectangle(moveableObject.Position, moveableObject.CollisionRectangle);

            if (rect1.Intersects(staticObject))
            {
                int differenceX = 0;
                int differenceY = 0;

                if (staticObject.Top < rect1.Bottom)
                {
                    differenceY = staticObject.Top + (rect1.Bottom - staticObject.Top);
                }
                if (rect1.Top < staticObject.Bottom)
                {
                    differenceY = rect1.Top + (staticObject.Bottom - rect1.Top);
                }

                if (staticObject.Left < rect1.Right)
                {
                    differenceX = staticObject.Left + (rect1.Right - staticObject.Left);
                }
                if (rect1.Left < staticObject.Right)
                {
                    differenceX = rect1.Left + (staticObject.Right - rect1.Left);
                }

                moveableObject.Position = new Vector2(
                    moveableObject.Position.X + differenceX,
                    moveableObject.Position.Y + differenceY);
            }
        }
    }
}
