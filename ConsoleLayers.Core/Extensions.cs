using System.Collections.Generic;
using System.Linq;

namespace ConsoleLayers.Core
{
    public static class Extensions
    {
        public static List<LocatedSymbol> MergeConsecutive(this IEnumerable<LocatedSymbol> locatedSymbols)
        {
            var ordered = locatedSymbols.OrderBy(locSymb => locSymb.GridX).ToList();
            var res = new List<LocatedSymbol>();

            int currentHead = 0;

            for (int i = 1; i < ordered.Count; i++)
            {
                if (ordered[currentHead].Symbol.IsSameForDrawing(ordered[i].Symbol))
                {
                    ordered[currentHead] = ordered[currentHead].MergeWith(ordered[i]);
                }
                else
                {
                    res.Add(ordered[currentHead]);
                    currentHead = i;
                }
            }

            res.Add(ordered[currentHead]);
            return res;
        }
    }
}
