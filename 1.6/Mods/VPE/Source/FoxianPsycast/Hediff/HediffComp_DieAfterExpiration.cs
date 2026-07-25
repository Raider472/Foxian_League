using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class HediffComp_DieAfterExpiration : HediffComp {
        public HediffCompProperties_DieAfterExpiration dieAfterExpirationProps => (HediffCompProperties_DieAfterExpiration)props;

        public override void CompPostPostRemoved() {
            base.CompPostPostRemoved();
            if(parent.pawn != null && !parent.pawn.Dead) {
                parent.pawn.Kill(null, parent);
            }
        }
    }
}
