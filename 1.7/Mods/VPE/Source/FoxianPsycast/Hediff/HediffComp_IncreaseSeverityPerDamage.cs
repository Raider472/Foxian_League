using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class HediffComp_IncreaseSeverityPerDamage : HediffComp {
        public HediffCompProperties_IncreaseSeverityPerDamage increaseSeverityPerDamageProps => (HediffCompProperties_IncreaseSeverityPerDamage)props;

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt) {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
            if(increaseSeverityPerDamageProps.severityIncreasePerDamage.TryGetValue(dinfo.Def, out FloatRange severityIncreaseRange)) {
                parent.Severity += severityIncreaseRange.RandomInRange;
            }
        }
    }
}
