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
                Log.Message($"Here is result of negativity interaction before: {__result}");
                __result *= ComposedSelectionChanceFactor;
                Log.Message($"Here is result of negativity interaction after: {__result}");
            }
            return;
        }
    }
}
