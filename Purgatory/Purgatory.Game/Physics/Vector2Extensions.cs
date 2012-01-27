using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Purgatory.Game.Physics
{
    public static class Vector2Extensions
    {
        public static bool ApproxEquals(this Vector2 v1, Vector2 v2)
        {
            if (Vector2.DistanceSquared(v1, v2) < 0.01)
            {
                return true;
            }

            return false;
        }
    }
}
