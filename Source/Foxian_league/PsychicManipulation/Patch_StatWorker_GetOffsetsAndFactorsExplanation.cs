using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [HarmonyPatch(typeof(StatWorker), "GetOffsetsAndFactorsExplanation", new Type[] { typeof(StatRequest), typeof(StringBuilder), typeof(float), typeof(string) })]
    public static class Patch_StatWorker_GetOffsetsAndFactorsExplanation {
        [HarmonyPostfix]
        public static void Postfix(StatWorker __instance, StatRequest req, StringBuilder sb, float baseValue, string whitespace) {
            if(req.HasThing && req.Thing != null && req.Thing is Pawn pawn && (!__instance.IsDisabledFor(req.Thing))) {
                StatDef stat = Traverse.Create(__instance).Field("stat").GetValue<StatDef>();
                if(Utils.HasActiveGene(pawn, InternalDefOf.FL_PsychicManipulation)) {
                    Gene_PsychicManipulation psychicManip = pawn.genes.GetGene(InternalDefOf.FL_PsychicManipulation) as Gene_PsychicManipulation;
                    if(stat == StatDefOf.NegotiationAbility && stat != null) {
                        sb.AppendLine(string.Format(whitespace + "{0}: {1}", "GeneLabelWithDesc".Translate(psychicManip.def.Named("GENE")).CapitalizeFirst(), psychicManip.negotiationAbility.ToStringPercentSigned()));
                    }
                    else if(stat == StatDefOf.TradePriceImprovement && stat != null) {
                        sb.AppendLine(string.Format(whitespace + "{0}: {1}", "GeneLabelWithDesc".Translate(psychicManip.def.Named("GENE")).CapitalizeFirst(), psychicManip.tradePriceImprovement.ToStringPercentSigned()));
                    }
                }
            }
        }
    }
}
