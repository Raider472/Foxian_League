using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class ComProperties_AddHediffOnEquip : CompProperties {
        public HediffDef hediffOnEquip;

        public ComProperties_AddHediffOnEquip() {
            compClass = typeof(Comp_AddHediffOnEquip);
        }
    }
}
