using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Comp_AddHediffOnEquip : ThingComp {

        public ComProperties_AddHediffOnEquip Props => (ComProperties_AddHediffOnEquip)props;

        public override void Notify_Equipped(Pawn pawn) {
            if (pawn == null || Props.hediffOnEquip == null) return;
            base.Notify_Equipped(pawn);
            if (pawn.health.hediffSet.TryGetHediff(Props.hediffOnEquip, out Hediff hediff)) {
                Log.Message("This pawn already has the hediff");
                return;
            }
            Hediff hediffToAdd = HediffMaker.MakeHediff(Props.hediffOnEquip, pawn, null);
            pawn.health.AddHediff(hediffToAdd);
        }

        public override void Notify_KilledPawn(Pawn pawn) {
            //Pawn is the pawn who killed
            if (pawn == null) return;
            base.Notify_KilledPawn(pawn);
            LocalTargetInfo latestTarget = pawn.LastAttackedTarget;
            Log.Message($"The killed Pawn is: {latestTarget}");
        }

        public override void Notify_Unequipped(Pawn pawn) {
            if (pawn == null) return;
            base.Notify_Unequipped(pawn);
            if (pawn.health.InPainShock) {
                Log.Message("Help this pawn plssssssss");
            }
            else {
                Log.Message("He did this on his own");
            }
        }
    }
}
