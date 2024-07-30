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
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawnRelations")]
    public static class PawnGenerator_GeneratePawnRelations_Patch {
        [HarmonyPrefix]
        public static bool DisableRelations(Pawn pawn) {
            if (pawn.HasActiveGene(InternalDefOf.FL_Female)) {
                return false;
            }
            else return true;


        }
    }
}
