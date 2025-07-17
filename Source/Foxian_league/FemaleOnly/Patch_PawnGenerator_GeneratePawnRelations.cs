using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using RimWorld;

namespace Foxian_league {
    //I guess a patch to fix social relations when this gene is applied
    //Code copied from "Male- and Female- Only Genes Continued"
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawnRelations")]
    public static class Patch_PawnGenerator_GeneratePawnRelations_Patch {
        [HarmonyPrefix]
        public static bool PreFix(Pawn pawn) {
            if (Utils.HasActiveGene(pawn, InternalDefOf.FL_Female)) {
                return false;
            }
            else return true;


        }
    }
}
