using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using static HarmonyLib.Code;

namespace Foxian_league {
    //Patch to apply the malus of the trait "Natural psy-sensitivity"
    //Check if the comp exists
    //If the building is not a natural one, then the building is -25% effective
    [HarmonyPatch(typeof(MeditationUtility), "PsyfocusGainPerTick", new Type[] { typeof(Pawn), typeof(Thing) })]
    public static class Patch_MediationUtility_PsyfocusGainPerTick {

        [HarmonyPostfix]
        public static void PostFix(ref float __result, Pawn pawn, Thing focus) {
            if (pawn == null || focus == null) return;
            if (pawn.story.traits.HasTrait(InternalDefOf.FL_NaturalPsySensitive_Trait) && focus != null) {
                CompMeditationFocus medidationComp = focus.TryGetComp<CompMeditationFocus>();
                if (medidationComp != null && focus.HasComp<CompMeditationFocus>() && !medidationComp.Props.focusTypes.Contains(MeditationFocusDefOf.Natural)) {
                    __result *= 0.75f;
                }
            }
            return;
        }
    }
}
