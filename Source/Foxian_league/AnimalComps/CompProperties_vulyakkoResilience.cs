using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class CompProperties_vulyakkoResilience : CompProperties {
        //Comp prop for setting the chance of procing and how much to resist incoming damage
        public float chanceToProc;
        public float minPercentageReduction;
        public float maxPercentageReduction;
        public CompProperties_vulyakkoResilience() {
            compClass = typeof(Comp_vulyakkoResilience);
        }
    }
}
