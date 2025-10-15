using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Foxian_league {
    //Patch to save the Pawn who started the taming attempt if it has the corresponding gene, and the animal undergoing that process 
    [HarmonyPatch(typeof(Pawn_MindState), "CheckStartMentalStateBecauseRecruitAttempted", new Type[] { typeof(Pawn) })]
    public static class Patch_Pawn_MindState_CheckStartMentalStateBecauseRecruitAttempted {
        public static Pawn actualTamer;
        public static Pawn actualAnimal;

        [HarmonyPrefix]
        public static void PreFix(Pawn tamer, Pawn_MindState __instance) {
            if (tamer == null) return;
            if (Utils.HasActiveGene(tamer, InternalDefOf.FL_AnimalConnection)) {
                actualTamer = tamer;
                actualAnimal = __instance.pawn;
                Log.Message($"Actual Tamer: {actualTamer} and actual animal: {actualAnimal}");
            }
            return;
        }

        [HarmonyPostfix]
        public static void PostFix() {
            if (actualAnimal != null) actualAnimal = null;
            if (actualTamer != null) actualTamer = null;
            return;
        }
    }
}
