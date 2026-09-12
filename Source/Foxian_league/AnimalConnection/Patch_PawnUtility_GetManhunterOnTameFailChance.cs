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
        private static readonly SimpleCurve manhunterReductionCurve = new SimpleCurve {
            new CurvePoint(1f, 1f),
            new CurvePoint(1.5f, 0.9f),
            new CurvePoint(2f, 0.85f),
            new CurvePoint(2.25f, 0.8f),
            new CurvePoint(2.5f, 0.75f),
        };

        [HarmonyPostfix]
        public static void PostFix(Pawn pawn, ref float __result) {
            if (pawn == null) return;
            if (__result == 0) return;
            if (pawn == Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted.actualAnimal) {
                float reductionCurve = GetManhunterReductionBasedOnTamerPsychic(Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted.actualTamer);
                Log.Message($"Tamer: {Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted.actualTamer.Name}, psychic sensitivity: {Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted.actualTamer.GetStatValue(StatDefOf.PsychicSensitivity)}");
                Log.Message($"[Animal Connection] ManhunterOnTameFailChance for {pawn.Name} reduced by {1f - reductionCurve * 100}% due to tamer's psychic sensitivity, value of result before calculation: {__result}");
                __result *= reductionCurve;
                Log.Message($"[Animal Connection] ManhunterOnTameFailChance for {pawn.Name} after calculation: {__result}");
            }
            return;
        }

        private static float GetManhunterReductionBasedOnTamerPsychic(Pawn tamer) {
            return manhunterReductionCurve.Evaluate(tamer.GetStatValue(StatDefOf.PsychicSensitivity));
        }
    }
}
