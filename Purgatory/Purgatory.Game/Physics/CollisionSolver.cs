
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

        public static void SolveCollision(IMoveable moveableObject, IStatic staticObject)
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

        public static void SolveCollision(IMoveable moveableObject, Rectangle staticObject)
        {
            Rectangle rect1 = GeometryUtility.GetAdjustedRectangle(moveableObject.Position, moveableObject.CollisionRectangle);

            if (rect1.Intersects(staticObject))
            {
                Vector2 movement = moveableObject.Position - moveableObject.LastPosition;
                Vector2 penetration = Vector2.Zero;
                if (movement.X > 0)
                {
                    penetration.X = rect1.Right - staticObject.Left;
                    if (penetration.X <= 0 || penetration.X > movement.X + 1)
                    {
                        penetration.X = 0;
                    }
                }
                else if (movement.X < 0)
                {
                    penetration.X = rect1.Left - staticObject.Right;
                    if (penetration.X >= 0 || penetration.X < movement.X - 1)
                    {
                        penetration.X = 0;
                    }
                }

                if (movement.Y > 0)
                {
                    penetration.Y = rect1.Bottom - staticObject.Top;
                    if (penetration.Y <= 0 || penetration.Y > movement.Y + 1)
                    {
                        penetration.Y = 0;
                    }
                }
                else if (movement.Y < 0)
                {
                    penetration.Y = rect1.Top - staticObject.Bottom;
                    if (penetration.Y >= 0 || penetration.Y < movement.Y - 1)
                    {
                        penetration.Y = 0;
                    }
                }

                moveableObject.Position -= penetration;
                //Vector2 movementVec = moveableObject.Position - moveableObject.LastPosition;
                //Vector2 offset = Vector2.Zero;

                //if (movementVec.X > 0)
                //{
                //    offset.X = moveableObject.CollisionRectangle.Width / 2;
                //}
                //else if (movementVec.X < 0)
                //{
                //    offset.X = -moveableObject.CollisionRectangle.Width / 2;
                //}

                //if (movementVec.Y > 0)
                //{
                //    offset.Y = moveableObject.CollisionRectangle.Height / 2;
                //}
                //else if (movementVec.Y < 0)
                //{
                //    offset.Y = -moveableObject.CollisionRectangle.Height / 2;
                //}

                //Line movement = new Line() { P1 = moveableObject.LastPosition + offset, P2 = moveableObject.Position + offset };

                //Line l1 = new Line() { P1 = new Vector2(staticObject.Left, staticObject.Top), P2 = new Vector2(staticObject.Right, staticObject.Top) };
                //Line l2 = new Line() { P1 = new Vector2(staticObject.Right, staticObject.Top), P2 = new Vector2(staticObject.Right, staticObject.Bottom) };
                //Line l3 = new Line() { P1 = new Vector2(staticObject.Right, staticObject.Bottom), P2 = new Vector2(staticObject.Left, staticObject.Bottom) };
                //Line l4 = new Line() { P1 = new Vector2(staticObject.Left, staticObject.Bottom), P2 = new Vector2(staticObject.Left, staticObject.Top) };

                //Vector2 closestPosition = new Vector2(float.MaxValue);
                //float closestDistanceSquared = float.MaxValue;

                //Vector2? collision = movement.IntersectsLineSegment(l1);
                //if (collision.HasValue)
                //{
                //    Vector2 newPosition = collision.Value - offset;
                //    float distanceSquared = Vector2.DistanceSquared(moveableObject.LastPosition, newPosition);
                //    if (distanceSquared < closestDistanceSquared)
                //    {
                //        closestDistanceSquared = distanceSquared;
                //        closestPosition = newPosition;
                //    }
                //}

                //collision = movement.IntersectsLineSegment(l2);
                //if (collision.HasValue)
                //{
                //    Vector2 newPosition = collision.Value - offset;
                //    float distanceSquared = Vector2.DistanceSquared(moveableObject.LastPosition, newPosition);
                //    if (distanceSquared < closestDistanceSquared)
                //    {
                //        closestDistanceSquared = distanceSquared;
                //        closestPosition = newPosition;
                //    }
                //}

                //collision = movement.IntersectsLineSegment(l3);
                //if (collision.HasValue)
                //{
                //    Vector2 newPosition = collision.Value - offset;
                //    float distanceSquared = Vector2.DistanceSquared(moveableObject.LastPosition, newPosition);
                //    if (distanceSquared < closestDistanceSquared)
                //    {
                //        closestDistanceSquared = distanceSquared;
                //        closestPosition = newPosition;
                //    }
                //}

                //collision = movement.IntersectsLineSegment(l4);
                //if (collision.HasValue)
                //{
                //    Vector2 newPosition = collision.Value - offset;
                //    float distanceSquared = Vector2.DistanceSquared(moveableObject.LastPosition, newPosition);
                //    if (distanceSquared < closestDistanceSquared)
                //    {
                //        closestDistanceSquared = distanceSquared;
                //        closestPosition = newPosition;
                //    }
                //}

                //moveableObject.Position = closestPosition;

                //if (movementLastFrame.X < 0)
                //{
                //    movementX = (int)Math.Floor(movementLastFrame.X);
                //}
                //else
                //{
                //    movementX = (int)Math.Ceiling(movementLastFrame.X);
                //}

                //if (movementLastFrame.Y < 0)
                //{
                //    movementY = (int)Math.Floor(movementLastFrame.Y);
                //}
                //else
                //{
                //    movementY = (int)Math.Ceiling(movementLastFrame.Y);
                //}

                //int differenceX = 0;
                //int differenceY = 0;

                //if (rect1.Bottom > staticObject.Top && rect1.Bottom - movementY <= staticObject.Top)
                //{
                //    differenceY = rect1.Bottom - staticObject.Top;
                //}
                //else if (rect1.Top < staticObject.Bottom && rect1.Top - movementY >= staticObject.Bottom)
                //{
                //    differenceY = staticObject.Bottom - rect1.Top;
                //}

                //if (rect1.Right > staticObject.Left && rect1.Right - movementX <= staticObject.Left)
                //{
                //    differenceX = rect1.Right - staticObject.Left;
                //}
                //else if (rect1.Left < staticObject.Right && rect1.Left - movementX >= staticObject.Right)
                //{
                //    differenceX = staticObject.Right - rect1.Left;
                //}

                //moveableObject.Position = new Vector2(
                //    moveableObject.Position.X - differenceX,
                //    moveableObject.Position.Y - differenceY);
            }
        }
    }
}
