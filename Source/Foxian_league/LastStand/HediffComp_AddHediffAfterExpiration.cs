using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class HediffComp_AddHediffAfterExpiration : HediffComp {
        //Comp that apply an hediff when it is removed
        public HediffCompProperties_AddHediffAfterExpiration expirationProps => (HediffCompProperties_AddHediffAfterExpiration)props;

        public override void CompPostPostRemoved() {
            base.CompPostPostRemoved();
            if(parent.pawn.health.State != PawnHealthState.Dead) {
                parent.pawn.health.AddHediff(expirationProps.hediffToGive);
            }
        }
    }
}
