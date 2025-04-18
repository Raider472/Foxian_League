using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [HarmonyPatch(typeof(PregnancyUtility), "ApplyBirthOutcome")]
    public static class Patch_PregnancyUtility_ApplyBirthOutcome {
        public static Pawn mother;

        [HarmonyPrefix]
        public static void Prefix(Pawn geneticMother) {
            if (Utils.HasActiveGene(geneticMother, InternalDefOf.FL_GeneticPurity)) {
                mother = geneticMother;
            }
        }

        [HarmonyPostfix]
        public static void Postfix() {
            if(mother != null) {
                mother = null;
            }
        }
    }
}
