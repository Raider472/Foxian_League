using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class HediffCompProperties_DieAfterExpiration : HediffCompProperties {
        public HediffCompProperties_DieAfterExpiration() {
            compClass = typeof(HediffComp_DieAfterExpiration);
        }
    }
}
