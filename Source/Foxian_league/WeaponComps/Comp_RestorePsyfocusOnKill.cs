using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Comp_RestorePsyfocusOnKill : ThingComp {
        public CompProperties_RestorePsyfocusOnKill Props => (CompProperties_RestorePsyfocusOnKill)props;

        public override void Notify_KilledPawn(Pawn pawn) {
            if (pawn == null || !pawn.psychicEntropy.NeedsPsyfocus) return;
            LocalTargetInfo latestTarget = pawn.LastAttackedTarget;
            if (latestTarget == null) return;
            base.Notify_KilledPawn(pawn);

            if(latestTarget.Pawn.RaceProps.IsMechanoid && !Props.restoreOnNonFleshKill) {
                Log.Message($"Not restoring psyfocus to {pawn.Name} for killing {latestTarget.Thing?.Label ?? "unknown target"} because it is a mechanoid and restoreOnNonFleshKill is false.");
                return;
            }
            float psyFocusToRestore = CalculatePsyFocus(Props.psyfocusRestoreRange);
            Log.Message($"Psyfocus Before: {pawn.psychicEntropy.CurrentPsyfocus}");
            pawn.psychicEntropy.OffsetPsyfocusDirectly(psyFocusToRestore);
            Log.Message($"Restoring {psyFocusToRestore} psyfocus to {pawn.Name} for killing {latestTarget.Thing?.Label ?? "unknown target"}. and final psyfocus: {pawn.psychicEntropy.CurrentPsyfocus}");

        }

        private float CalculatePsyFocus(FloatRange psyfocusRestoreRange) {
            return psyfocusRestoreRange.RandomInRange;
        }
    }
}
