using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Class to give social bonus/malus to other pawns depending on their certainty if the initiator has the "Loyal" gene
    public class ThoughtWorker_LoyalSocial : ThoughtWorker {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn) {
            Ideo ideoPawn = p.ideo.Ideo;
            Ideo ideoOtherPawn = otherPawn.ideo.Ideo;

            if (!p.RaceProps.Humanlike) {
                return false;
            }
            if (!p.story.traits.HasTrait(InternalDefOf.FL_Loyal_Trait)) {
                return false;
            }
            if (p.ideo.Certainty < 0.50) {
                return false;
            }
            if (ideoPawn != ideoOtherPawn) {
                return ThoughtState.ActiveAtStage(1);
            }
            else if (ideoPawn == ideoOtherPawn) {
                if (otherPawn.ideo.Certainty >= 0.80) {
                    return ThoughtState.ActiveAtStage(2);
                }
                else if (otherPawn.ideo.Certainty < 0.50) {
                    return ThoughtState.ActiveAtStage(0);
                }
            }
            return false;
        }
    }
}
