using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using RimWorld;

namespace Foxian_league
{
    public static class Utils {
        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef) {
            if (geneDef is null) return false;
            if (pawn.genes is null) return false;
            return pawn.genes.GetGene(geneDef)?.Active ?? false;
        }
    }
}
