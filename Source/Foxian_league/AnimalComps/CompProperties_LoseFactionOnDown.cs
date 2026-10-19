using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class CompProperties_LoseFactionOnDown: CompProperties {
        //Comp prop to configure which faction lose this animal when it become downed in Raids
        public FactionDef factionToLose;
        public bool automaticallyLoseFactionOnDown = true;
        public float chanceToLoseFactionOnDown = 0f;

        public CompProperties_LoseFactionOnDown() {
            compClass = typeof(Comp_LoseFactionOnDown);
        }
    }
}
