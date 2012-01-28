using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Purgatory.Game
{
    public class Randomiser : Comparer<Tuple<int, int>>
    {
        public override int Compare(Tuple<int, int> x, Tuple<int, int> y)
        {
            return new Random().Next(-1, 1);
        }
    }
}
