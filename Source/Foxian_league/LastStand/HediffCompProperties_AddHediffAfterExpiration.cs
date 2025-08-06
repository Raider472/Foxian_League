using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class HediffCompProperties_AddHediffAfterExpiration : HediffCompProperties {
        //CompProp for the apply hediff when removed
        public HediffDef hediffToGive;
        public HediffCompProperties_AddHediffAfterExpiration() {
            compClass = typeof(HediffComp_AddHediffAfterExpiration);
        }
    }
}
