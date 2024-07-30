using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace Foxian_league {
    [DefOf]
    public static class InternalDefOf {
        public static GeneDef FL_Female;

        static InternalDefOf() {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }
    }
}
