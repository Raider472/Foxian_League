using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using RimWorld;
using UnityEngine.Analytics;

namespace Foxian_league {
    public static class Utils {
        //A bunch of utility methods to avoid code duplication
        public static bool HasActiveGene(Pawn pawn, GeneDef geneDef) {
            if (pawn.genes is null || geneDef is null) {
                return false;
            }
            bool? isGeneActive = pawn.genes.GetGene(geneDef)?.Active;
            if (isGeneActive == true) { 
                return true;
            }
            return false;
        }

        public static List<Gene> GetAllGenesFromMother(Pawn mother) {
            if (mother == null) return new List<Gene>();
            List<Gene> motherGenes = new List<Gene>();
            motherGenes.AddRange(mother.genes.Xenogenes);
            motherGenes.AddRange(mother.genes.Endogenes);
            return motherGenes;
        }

        public static List<GeneDef> ClearNonCosmeticGeneBaby(List<GeneDef> babyGenes) {
            if (babyGenes == null) return new List<GeneDef>();
            List<GeneDef> filteredGenes = new List<GeneDef>();
            for (int i = 0; i < babyGenes.Count; i++) {
                if (babyGenes[i].endogeneCategory == EndogeneCategory.Melanin || babyGenes[i].endogeneCategory == EndogeneCategory.HairColor) {
                    filteredGenes.Add(babyGenes[i]);
                }
            }
            return filteredGenes;
        }

        public static bool IsFoxian(Pawn pawn) {
            return pawn.genes.Xenotype == InternalDefOf.FL_Foxian;
        }

        public static bool IsGreaterFoxian(Pawn pawn) {
            return pawn.genes.Xenotype == InternalDefOf.FL_Greater_Foxian;
        }

        public static bool IsPawnFoxianEnough(Pawn pawn) {
            if(pawn == null) return false;
            Dictionary<string, bool> dictGeneList = new Dictionary<string, bool>() {
                { "FL_FoxEar", false },
                { "FL_FoxTail", false },
                { "FL_NaturalPsySensitive", false },
                { "FL_Reserved", false },
                { "FL_Composed", false },
                { "FL_Emotionally_Brittle", false },
                { "FireWeakness", false }
            };

            //List<Gene> geneList = [.. pawn.genes.Endogenes, .. pawn.genes.Xenogenes];
            List<Gene> geneList = GetAllGenesFromMother(pawn);

            for (int i = 0; i < geneList.Count; i++) {
                string nameGeneDef = geneList[i].def.defName;
                if(nameGeneDef == "FL_NineFoxTail" && geneList[i].Active) {
                    dictGeneList["FL_FoxTail"] = true;
                    continue;
                }
                if (dictGeneList.ContainsKey(nameGeneDef) && geneList[i].Active) {
                    dictGeneList[nameGeneDef] = true;
                }
            }
            return !dictGeneList.ContainsValue(false) ;
        }
    }
}
