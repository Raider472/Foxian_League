using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class ComProperties_AddHediffOnEquip : CompProperties {
        //Comp to add an hediff on equip, alt hediff is for leaders so they don't die when having this sword
        public HediffDef hediffOnEquip;
        public HediffDef hediffOnEquipAlt;

        public ComProperties_AddHediffOnEquip() {
            compClass = typeof(Comp_AddHediffOnEquip);
        }
    }
}
