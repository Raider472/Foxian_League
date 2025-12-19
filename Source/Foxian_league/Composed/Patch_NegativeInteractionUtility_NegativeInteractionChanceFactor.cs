using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Patch to add the Composed case for the negative interaction chance factor
    //This will lower the odds of negative interactions
    [HarmonyPatch(typeof(NegativeInteractionUtility), "NegativeInteractionChanceFactor")]
    public static class Patch_NegativeInteractionUtility_NegativeInteractionChanceFactor {

        public const float ComposedSelectionChanceFactor = 0.5f;

        [HarmonyPostfix]
        public static void PostFix(ref float __result, Pawn initiator) {
            if (initiator == null) return;
            if (Utils.HasActiveGene(initiator, InternalDefOf.FL_Composed)) {
                __result *= ComposedSelectionChanceFactor;
            }
            return;
        }
    }
}
