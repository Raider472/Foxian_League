﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //By default, the game will think that a parent with the genetic purity gene will birth a "Hybrid"
    //So I've added a special case in this method for mother with the genetic purity gene
    [HarmonyPatch(typeof(PregnancyUtility), "TryGetInheritedXenotype")]
    public static class Patch_PregnancyUtility_TryGetInheritedXenotype {

        [HarmonyPostfix]
        public static void PostFix(Pawn mother, ref XenotypeDef xenotype, ref bool __result) {
            if(Utils.HasActiveGene(mother, InternalDefOf.FL_GeneticPurity) && (Utils.IsPawnFoxianEnough(mother) || Utils.IsFoxian(mother))) {
                if(Patch_PawnGenerator_GeneratePawn.isBabyGreaterFoxian) {
                    xenotype = InternalDefOf.FL_Greater_Foxian;
                }
                else {
                    xenotype = mother.genes?.Xenotype;
                }
                __result = true;
                Log.Message($"Here is current xenotype after tryGetInheritedXenotype: {xenotype} + if is greater foxian: {Patch_PawnGenerator_GeneratePawn.isBabyGreaterFoxian}");
            }
        }
    }
}
