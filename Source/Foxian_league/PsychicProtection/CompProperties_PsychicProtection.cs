using Foxian_league;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class CompProperties_PsychicProtection : CompProperties_AbilityGiveHediff {
        public HediffDef defaultHediff;
        public HediffDef alternatetHediff;

        public string defaultNameHediff = "FL_PsychichProtectionTank";
        public string AlternateNameHediff = "FL_PsychichProtectionDodge";

        public CompProperties_PsychicProtection() {
            compClass = typeof(Comp_PsychicProtection);
        }
    }
}
