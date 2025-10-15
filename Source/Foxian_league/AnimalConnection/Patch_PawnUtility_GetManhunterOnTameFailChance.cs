using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //This Patch is reliant on "Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted" to work correctly
    //Patch to change the return value of the ManhunterOnTameFailChance if the tamer has +150% psychic sensitivity + the gene, and if the animal is the same as the one saved in the other patch
    [HarmonyPatch(typeof(PawnUtility), "GetManhunterOnTameFailChance", new Type[] { typeof(Pawn) })]
    public static class Patch_PawnUtility_GetManhunterOnTameFailChance {
        [HarmonyPostfix]
        public static void PostFix(Pawn pawn, ref float __result) {
            if (pawn == null) return;
            if (__result == 0) return;
            if (pawn == Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted.actualAnimal && Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted.actualTamer.GetStatValue(StatDefOf.PsychicSensitivity) >= Foxian_Settings.animalConnectionFactorTrigger) {
                __result *= 0.75f;
                Log.Message($"Change when pawn has gene: {__result} and pawn is: {pawn}");
            }
            return;
        }
    }
}
