using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public static class HediffUtils {
        //A bunch of utility methods use to avoid repetition for the genes using hediff to work
        static public void SetHediffStage(string hediffName, int stage, Pawn pawn) {
            if (pawn != null) {
                HediffDef hediffDef = HediffDef.Named($"{hediffName}{stage}");
                hediffDef.initialSeverity = float.Epsilon;
                pawn.health.AddHediff(hediffDef);
            }
            return;
        }

        static public void RemoveHediffStage(string hediffName, int stage, Pawn pawn) {
            if (pawn != null) {
                Hediff hediffToRemove = pawn.health.hediffSet.hediffs.Find(x => x.def.defName == $"{hediffName}{stage}");
                if (hediffToRemove != null) {
                    pawn.health.RemoveHediff(hediffToRemove);
                }
                return;
            }
            return;
        }

        static public void RemoveHediffWithString(string hediffName, Pawn pawn) {
            if (pawn != null) {
                List<Hediff> hediffToRemoveList = pawn.health.hediffSet.hediffs.Where(x => x.def.defName.StartsWith($"{hediffName}")).ToList();
                if (hediffToRemoveList != null) {
                    foreach (Hediff hediff in hediffToRemoveList) {
                        pawn.health.RemoveHediff(hediff);
                    }
                }
                return;
            }
            return;
        }

    }
}
