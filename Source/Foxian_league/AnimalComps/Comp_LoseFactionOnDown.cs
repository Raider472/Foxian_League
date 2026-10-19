using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Comp_LoseFactionOnDown: ThingComp {
        //Comp to make the animal lose its specific faction when it become downed in Raids
        public CompProperties_LoseFactionOnDown Props => (CompProperties_LoseFactionOnDown)props;

        public override void Notify_Downed() {
            base.Notify_Downed();
            if(parent.Faction != null && parent.Faction.def == Props.factionToLose) {
                if(Props.automaticallyLoseFactionOnDown) {
                    parent.SetFaction(null);
                }
                else if(Rand.Chance(Props.chanceToLoseFactionOnDown)) {
                    parent.SetFaction(null);
                }
            }
        }
    }
}
