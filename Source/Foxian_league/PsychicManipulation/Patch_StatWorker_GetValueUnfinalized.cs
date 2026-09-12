using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [HarmonyPatch(typeof(StatWorker), "GetValueUnfinalized", new Type[] { typeof(StatRequest), typeof(bool) })]
    public static class Patch_StatWorker_GetValueUnfinalized {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, StatWorker __instance, StatRequest req) {
            if(req.HasThing && req.Thing != null && req.Thing is Pawn pawn && (!__instance.IsDisabledFor(req.Thing))) {
                StatDef stat = Traverse.Create(__instance).Field("stat").GetValue<StatDef>();
                if(Utils.HasActiveGene(pawn, InternalDefOf.FL_PsychicManipulation)) {
                    Gene_PsychicManipulation psychicManip = pawn.genes.GetGene(InternalDefOf.FL_PsychicManipulation) as Gene_PsychicManipulation;
                    if(stat == StatDefOf.NegotiationAbility && stat != null) {
                        Log.Message($"[Foxian League] Postfix called for {pawn.Name} with stat {stat.defName}");
                        Log.Message($"[Foxian League] NegotiationAbility before modification: {__result}, modification: {psychicManip.negotiationAbility}");
                        __result += psychicManip.negotiationAbility;
                        Log.Message($"[Foxian League] Postfix result for {pawn.Name} with stat {stat.defName} is {__result}");
                    }
                    else if(stat == StatDefOf.TradePriceImprovement && stat != null) {
                        Log.Message($"[Foxian League] Postfix called for {pawn.Name} with stat {stat.defName}");
                        Log.Message($"[Foxian League] TradePriceImprovement before modification: {__result}, modification: {psychicManip.tradePriceImprovement}");
                        __result += psychicManip.tradePriceImprovement;
                        Log.Message($"[Foxian League] Postfix result for {pawn.Name} with stat {stat.defName} is {__result}");
                    }
                }
            }
        }
    }
}
