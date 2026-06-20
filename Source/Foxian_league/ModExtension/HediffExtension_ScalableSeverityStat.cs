using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class HediffExtension_ScalableSeverityStat : DefModExtension {
        public StatDef scalingStat;
        public FloatRange statLimit;
        public bool shouldBeRemovedWhenZero = false;
    }
}
