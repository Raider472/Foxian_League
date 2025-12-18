using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Hediff_clotting : HediffWithComps {
        //Yes, this comp is basically super clotting but as a hediff
        private static readonly FloatRange TendingQualityRange = new FloatRange(0.2f, 0.7f);

        public override void PostTickInterval(int delta) {
            base.PostTickInterval(delta);
            if(!pawn.IsHashIntervalTick(360, delta)) {
                return;
            }
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for(int num = hediffs.Count - 1; num >= 0; num--) {
                if(hediffs[num].Bleeding) {
                    hediffs[num].Tended(TendingQualityRange.RandomInRange, TendingQualityRange.TrueMax, 1);
                }
            }
        }
    }
}
