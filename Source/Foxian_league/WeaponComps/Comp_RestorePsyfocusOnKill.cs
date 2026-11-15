using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    public class Comp_RestorePsyfocusOnKill : ThingComp {
        public CompProperties_RestorePsyfocusOnKill Props => (CompProperties_RestorePsyfocusOnKill)props;

        public override void Notify_KilledPawn(Pawn pawn) {
            if (pawn == null || pawn.psychicEntropy == null || !pawn.psychicEntropy.NeedsPsyfocus) return;

            LocalTargetInfo latestTarget = pawn.LastAttackedTarget;
            if(!latestTarget.IsValid || !latestTarget.HasThing) return;

            base.Notify_KilledPawn(pawn);

            if(latestTarget.Pawn.RaceProps.IsMechanoid && !Props.restoreOnNonFleshKill) {
                return;
            }
            float psyFocusToRestore = CalculatePsyFocus(Props.psyfocusRestoreRange, latestTarget.Pawn.RaceProps.baseHealthScale);
            pawn.psychicEntropy.OffsetPsyfocusDirectly(psyFocusToRestore);

        }

        private float CalculatePsyFocus(FloatRange psyfocusRestoreRange, float healthScale) {
            return Mathf.Clamp(psyfocusRestoreRange.RandomInRange * healthScale, 0f, 1f);
        }
    }
}
