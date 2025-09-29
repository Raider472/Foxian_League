using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Pawns with the genetic purity gene should not be affected by this, the mods will call greater foxian as hybrid foxian and it throws an error (the error seems benign)
    [HarmonyPatch]
    static class BGI_ApplyBirthOutcome {
        [HarmonyPrepare]
        public static bool Prepare() {
            return ModsConfig.IsActive("RedMattis.BetterGeneInheritance");
        }

        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() {
            return AccessTools.Method("BGInheritance.HarmonyPatches:ApplyBirthOutcomePostfix");
        }

        [HarmonyPrefix]
        public static bool Prefix(Pawn geneticMother) {
            if (geneticMother == null || !Utils.HasActiveGene(geneticMother, InternalDefOf.FL_GeneticPurity)) return true;
            Log.Message("Genetic mother had the genes - BGI Edition");
            return false;
        }
    }
}
