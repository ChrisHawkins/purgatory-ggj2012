
namespace Purgatory.Game.Physics
{
    using System;
    using Microsoft.Xna.Framework;

    public static class CollisionSolver
    {
        public static void SolveCollision(IMoveable moveableObject, IMoveable moveableObject2)
        {
            throw new NotImplementedException("Moveable can't hit moveable");
        }

        public static Vector2 SolveCollision(IMoveable moveableObject, IStatic staticObject)
        {
            Rectangle rect1 = GeometryUtility.GetAdjustedRectangle(moveableObject.Position, moveableObject.CollisionRectangle);
            Rectangle rect2 = GeometryUtility.GetAdjustedRectangle(staticObject.Position, staticObject.CollisionRectangle);

            if (rect1.Intersects(rect2))
            {
                Vector2 movement = moveableObject.Position - moveableObject.LastPosition;
                Vector2 penetration = Vector2.Zero;
                if (movement.X > 0)
                {
                    penetration.X = rect1.Right - rect2.Left;
                    if (penetration.X <= 0 || penetration.X > movement.X + 1)
                    {
                        penetration.X = 0;
                    }
                }
                else if (movement.X < 0)
                {
                    penetration.X = rect1.Left - rect2.Right;
                    if (penetration.X >= 0 || penetration.X < movement.X - 1)
                    {
                        penetration.X = 0;
                    }
                }

                if (movement.Y > 0)
                {
                    penetration.Y = rect1.Bottom - rect2.Top;
                    if (penetration.Y <= 0 || penetration.Y > movement.Y + 1)
                    {
                        penetration.Y = 0;
                    }
                }
                else if (movement.Y < 0)
                {
                    penetration.Y = rect1.Top - rect2.Bottom;
                    if (penetration.Y >= 0 || penetration.Y < movement.Y - 1)
                    {
                        penetration.Y = 0;
                    }
                }

                return penetration;
            }

            return Vector2.Zero;
        }

        public static Vector2 SolveCollision(IMoveable moveableObject, Rectangle staticObject)
        {
            Rectangle rect1 = moveableObject.CollisionRectangle;

            if (rect1.Intersects(staticObject) || staticObject.Intersects(rect1))
            {
                Vector2 movement = moveableObject.Position - moveableObject.LastPosition;
                Vector2 penetration = Vector2.Zero;
                if (movement.X > 0)
                {
                    penetration.X = moveableObject.Position.X + rect1.Width / 2 - staticObject.Left;
                    if (penetration.X <= 0 || penetration.X > movement.X + 1)
                    {
                        penetration.X = 0;
                    }
                }
                else if (movement.X < 0)
                {
                    penetration.X = moveableObject.Position.X - rect1.Width / 2 - staticObject.Right;
                    if (penetration.X >= 0 || penetration.X < movement.X - 1)
                    {
                        penetration.X = 0;
                    }
                }

                if (movement.Y > 0)
                {
                    penetration.Y = moveableObject.Position.Y + rect1.Height / 2 - staticObject.Top;
                    if (penetration.Y <= 0 || penetration.Y > movement.Y + 1)
                    {
                        penetration.Y = 0;
                    }
                }
                else if (movement.Y < 0)
                {
                    penetration.Y = moveableObject.Position.Y - rect1.Height / 2 - staticObject.Bottom;
                    if (penetration.Y >= 0 || penetration.Y < movement.Y - 1)
                    {
                        penetration.Y = 0;
                    }
                }

                return penetration;
            }

            return Vector2.Zero;
        }
    }
}
