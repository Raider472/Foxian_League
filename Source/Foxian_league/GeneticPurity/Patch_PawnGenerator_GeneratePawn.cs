using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
    public static class Patch_PawnGenerator_GeneratePawn {

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
                    request.FixedGender = Gender.Female;
                    request.ForcedEndogenes = babyCosmeticGenes;
                    //TODO CHANGE WASTER TO GREATER FOX
                    request.ForcedXenotype = isBabyNormalFoxian ? InternalDefOf.Foxian : InternalDefOf.Waster;

                    if(!isBabyNormalFoxian ) {
                        request.FixedGender = Rand.Chance(0.8f) ? Gender.Male : Gender.Female;
                        Log.Message($"Baby Gender is: {request.FixedGender}");
                    }
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
                Patch_PregnancyUtility_ApplyBirthOutcome.mother = null;
            }
        }
    }
}
