using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //The purpose of this patch is to save the current birthing mother if she has the genetic purity gene
    //The field will be used to determine in the other patch if the baby has a mother with this specific gene or not
    //The reason is because the mother pawn is not a present parameter/field in the other method
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
