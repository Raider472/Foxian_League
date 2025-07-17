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
        public static XenotypeDef Foxian;
        public static XenotypeDef Waster;

        public static GeneDef FL_FoxEar;
        public static GeneDef FL_FoxTail;
        public static GeneDef FL_Reserved;
        public static GeneDef FL_PsychicManipulation;
        public static GeneDef FL_GeneticPurity;
        public static GeneDef FL_Female;
        public static GeneDef FL_Composed;
        public static GeneDef FL_Emotionally_Brittle;

        public static TraitDef FL_Reserved_Trait;
        public static TraitDef FL_Loyal_Trait;

        /*static InternalDefOf() {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }*/
    }
}
