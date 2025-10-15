using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class CompProperties_HediffOnHit : CompProperties {
        //Comp prop for adding an hediff on hit
        public HediffDef hediffOnHit;
        public float minValue;
        public float maxValue;

        public CompProperties_HediffOnHit() {
            compClass = typeof(Comp_HediffOnHit);
        }
    }
}
