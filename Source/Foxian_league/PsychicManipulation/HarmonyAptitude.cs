using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [HarmonyPatch(typeof(GeneDef), "AptitudeFor")]
    public static class HarmonyAptitude {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, GeneDef __instance, SkillDef skill) {
            if(__instance.HasModExtension<PsychicManipulation>()) {
                PsychicManipulation psychicManip = new PsychicManipulation(
                    __instance.GetModExtension<PsychicManipulation>().skillModifier,
                    __instance.GetModExtension<PsychicManipulation>().skillName
                );
                if (psychicManip.aptitude.skill == skill) {
                    __result += psychicManip.aptitude.level;
                }
            }
        }
    }
}
