using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Comp_HediffOnHit : ThingComp {

        public CompProperties_HediffOnHit Props => (CompProperties_HediffOnHit)props;
        public override string GetDescriptionPart() {
            return "Test Amongus gamingus";
        }

        public override void Notify_UsedWeapon(Pawn pawn) {
            if (pawn == null) return;
            base.Notify_UsedWeapon(pawn);
            LocalTargetInfo latestTarget = pawn.LastAttackedTarget;
            if (latestTarget != null && latestTarget.Pawn != null && !latestTarget.Pawn.IsShambler && !latestTarget.Pawn.RaceProps.IsMechanoid && !latestTarget.Pawn.Dead) {
                Hediff hediffToChange;
                Random rand = new Random();
                float randomValue = (float)(rand.NextDouble() * (Props.maxValue - Props.minValue) + Props.minValue);
                if (latestTarget.Pawn.health.hediffSet.TryGetHediff(Props.hediffOnHit, out hediffToChange)) {
                    hediffToChange.Severity += randomValue;
                    if (hediffToChange.Severity > 1f) {
                        hediffToChange.Severity = 1f;
                    }
                }
                else {
                    hediffToChange = HediffMaker.MakeHediff(Props.hediffOnHit, latestTarget.Pawn, null);
                    hediffToChange.Severity = randomValue;
                    latestTarget.Pawn.health.AddHediff(hediffToChange);
                }
            }
        }
    }
}
