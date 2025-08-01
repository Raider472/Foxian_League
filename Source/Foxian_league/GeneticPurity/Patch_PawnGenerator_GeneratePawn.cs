﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Patch to apply all the foxian traits to the child if the mother is a foxian (Behaviour A)
    //Otherwise the gene will use behaviour B if the mother has the gene but is not foxian enough
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
    public static class Patch_PawnGenerator_GeneratePawn {
        public static bool isBabyGreaterFoxian = false;

        [HarmonyPrefix]
        public static void Prefix(ref PawnGenerationRequest request) {
            Pawn mother = Patch_PregnancyUtility_ApplyBirthOutcome.mother;
            if (mother != null) {
                List<GeneDef> babyInheritedGenes = request.ForcedEndogenes;
                List<GeneDef> babyCosmeticGenes = Utils.ClearNonCosmeticGeneBaby(babyInheritedGenes);
                request.ForcedEndogenes.Clear();

                if (Utils.IsFoxian(mother) || Utils.IsPawnFoxianEnough(mother)) {
                    Log.Message("MOTHER IS FOXIAN");
                    bool isBabyNormalFoxian = Rand.Chance(0.7f);
                    request.ForcedEndogenes = babyCosmeticGenes;
                    request.ForcedXenotype = isBabyNormalFoxian ? InternalDefOf.FL_Foxian : InternalDefOf.FL_Greater_Foxian;
                    Log.Message($"forced custom xenotype: {request.ForcedCustomXenotype}");

                    if (!isBabyNormalFoxian ) {
                        isBabyGreaterFoxian = true;
                        request.FixedGender = Rand.Chance(0.7f) ? Gender.Male : Gender.Female;
                        Log.Message($"Baby Gender is: {request.FixedGender}");
                    }
                    else {
                        request.FixedGender = Gender.Female;
                    }
                    Log.Message($"forced after op xenotype: {request.ForcedXenotype}");
                }
                else {
                    Log.Message("MOTHER IS NOT FOXIAN");
                    request.ForcedEndogenes = babyCosmeticGenes;
                    request.FixedGender = Rand.Chance(0.8f) ? Gender.Female : Gender.Male;

                    if (mother.genes.HasEndogene(InternalDefOf.FL_GeneticPurity)) {
                        Log.Message("Mother has the Gene as Endogene");
                        for (int i = 0; i < mother.genes.Endogenes.Count; i++) {
                            request.AddForcedGene(mother.genes.Endogenes[i].def, false);
                            Log.Message($"{mother.genes.Endogenes[i].def}");
                        }
                    }
                    else {
                        Log.Message("Mother has the Gene as Xenogene");
                        for (int i = 0; i < mother.genes.Xenogenes.Count; i++) {
                            request.AddForcedGene(mother.genes.Xenogenes[i].def, false);
                            Log.Message($"{mother.genes.Xenogenes[i].def}");
                        }
                    }
                    Log.Message("GENERATION DONE");
                }
                Log.Message($"Is baby greater foxian ? {isBabyGreaterFoxian}");
            }
        }
    }
}
