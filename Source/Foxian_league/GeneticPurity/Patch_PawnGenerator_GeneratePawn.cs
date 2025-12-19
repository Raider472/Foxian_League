using HarmonyLib;
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
            if (Patch_PregnancyUtility_ApplyBirthOutcome.mother == null) return;
            Pawn mother = Patch_PregnancyUtility_ApplyBirthOutcome.mother;

            XenotypeDef xenotypeA = DefDatabase<XenotypeDef>.GetNamedSilentFail(Foxian_Settings.xenotypeA);
            XenotypeDef xenotypeB = DefDatabase<XenotypeDef>.GetNamedSilentFail(Foxian_Settings.xenotypeB);
            xenotypeA ??= InternalDefOf.FL_Foxian;
            xenotypeB ??= InternalDefOf.FL_Greater_Foxian;

            List<GeneDef> babyInheritedGenes = request.ForcedEndogenes;
            List<GeneDef> babyCosmeticGenes = Utils.ClearNonCosmeticGeneBaby(babyInheritedGenes);
            request.ForcedEndogenes.Clear();
            request.ForcedEndogenes = babyCosmeticGenes;

            if (Utils.IsFoxian(mother) || Utils.IsGreaterFoxian(mother) || Utils.IsPawnFoxianEnough(mother) || !Foxian_Settings.isPawnFoxianTrigger) {
                bool isBabyNormalFoxian = Rand.Chance(Foxian_Settings.xenotypeAChance);
                request.ForcedXenotype = isBabyNormalFoxian ? xenotypeA : xenotypeB;

                if (!isBabyNormalFoxian) {
                    isBabyGreaterFoxian = true;
                    request.FixedGender = Rand.Chance(Foxian_Settings.xenotypeMaleChance) ? Gender.Male : Gender.Female;
                }
                else {
                    request.FixedGender = Gender.Female;
                }
            }
            else {
                request.FixedGender = Rand.Chance(Foxian_Settings.xenotypeFemaleChanceAlt) ? Gender.Female : Gender.Male;

                if (mother.genes.HasEndogene(InternalDefOf.FL_GeneticPurity)) {
                    for (int i = 0; i < mother.genes.Endogenes.Count; i++) {
                        request.AddForcedGene(mother.genes.Endogenes[i].def, false);
                    }
                }
                else {
                    for (int i = 0; i < mother.genes.Xenogenes.Count; i++) {
                        request.AddForcedGene(mother.genes.Xenogenes[i].def, false);
                    }
                }
            }
            return;
        }
    }
}
