using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace FoxianPsycast {
    public class JobGiver_SacrificialTreeRitual: ThinkNode_JobGiver {
        protected override Job TryGiveJob(Pawn pawn) {
            PawnDuty duty = pawn.mindState.duty;
            if(duty == null) {
                return null;
            }
            if(!pawn.CanReserveAndReach(duty.focus, PathEndMode.OnCell, Danger.Deadly)) {
                return null;
            }
            if(duty.focusSecond.Thing?.def == null || duty.focusSecond.Thing.def != InternalDefOf.FL_Plant_SacrificialTree) {
                return null;
            }
            return JobMaker.MakeJob(InternalDefOf.FL_RitualSacrificialTree, duty.focusSecond, duty.focus);
        }
    }
}
