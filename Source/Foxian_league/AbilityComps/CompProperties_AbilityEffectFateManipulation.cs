using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foxian_league {
    public class CompProperties_AbilityEffectFateManipulation: CompProperties_AbilityEffect {
        public CompProperties_AbilityEffectFateManipulation() {
            this.compClass = typeof(CompAbilityEffect_FateManipulation);
        }
    }
}
