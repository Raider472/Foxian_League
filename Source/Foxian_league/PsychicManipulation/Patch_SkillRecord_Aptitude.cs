using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using Verse;

namespace Foxian_league {
    //Patch to add aptitude to the skill record if the pawn has the gene "Psychic manipulation"
    //Check if the gene is active, then check if skill modifier is superior to 0
    //Then add it to the attributes field to the corresponding skill
    [HarmonyPatch(typeof(SkillRecord), "Aptitude", MethodType.Getter)]
    public static class Patch_SkillRecord_Aptitude {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, SkillRecord __instance) {
            if(Utils.HasActiveGene(__instance.Pawn, InternalDefOf.FL_PsychicManipulation) && __instance.def == DefDatabase<SkillDef>.GetNamed("Social")) {
                Gene_PsychicManipulation psychicManip = __instance.Pawn.genes.GetGene(InternalDefOf.FL_PsychicManipulation) as Gene_PsychicManipulation;
                if(psychicManip != null && psychicManip.skillModifier != 0) {
                    __result += psychicManip.skillModifier;
                }
            }
        }

    }
}
