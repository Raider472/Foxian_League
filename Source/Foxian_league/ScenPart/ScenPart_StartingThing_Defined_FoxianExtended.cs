using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Class Extension to allow get and set a thingDef to make the medieval mode work
    public class ScenPart_StartingThing_Defined_FoxianExtended : ScenPart_StartingThing_Defined {
        public ThingDef ThingDef => thingDef;

        public void SetThingDef(ThingDef newThingDef) {
            thingDef = newThingDef;
        }

        public void setStuffDef(ThingDef newStuffDef) {
            stuff = newStuffDef;
        }
    }
}
