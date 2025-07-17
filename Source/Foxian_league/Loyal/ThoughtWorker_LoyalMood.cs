using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Class to switch mood for pawns with the gene "Loyal"
    public class ThoughtWorker_LoyalMood : ThoughtWorker {
        protected override ThoughtState CurrentStateInternal(Pawn p) {
            float certainty = p.ideo.Certainty;

            return certainty switch {
                >= 0 and < 0.25f => ThoughtState.ActiveAtStage(0),
                >= 0.25f and < 0.40f => ThoughtState.ActiveAtStage(1),
                >= 0.40f and < 0.50f => ThoughtState.ActiveAtStage(2),
                >= 0.80f and < 1f => ThoughtState.ActiveAtStage(3),
                _ => false
            };
        }
    }
}
