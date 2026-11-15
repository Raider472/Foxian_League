using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class HediffCompProperties_IncreaseSeverityPerDamage : HediffCompProperties {
        public Dictionary<DamageDef, FloatRange> severityIncreasePerDamage;

        public HediffCompProperties_IncreaseSeverityPerDamage() {
            compClass = typeof(HediffComp_IncreaseSeverityPerDamage);
        }
    }
}
