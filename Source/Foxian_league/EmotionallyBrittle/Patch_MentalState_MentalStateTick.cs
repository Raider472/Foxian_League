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
    //Patch to for making mental breakdowns last a little bit longer for pawns with the brittle gene
    //Mental breakdowns are ~33% longer
    //Could be done prefix but I feel like doing this in postfix will break less shit
    [HarmonyPatch(typeof(MentalState), "MentalStateTick", new Type[] { typeof(int) })]
    public static class Patch_MentalState_MentalStateTick {

        [HarmonyPostfix]
        public static void PostFix(MentalState __instance, int delta, ref int ___age) {
            if (Utils.HasActiveGene(__instance.pawn, InternalDefOf.FL_Emotionally_Brittle) && __instance.pawn.IsHashIntervalTick(30, delta)) {
                ___age -= 10;
            }
            return;
        }
    }
}
