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
        //Comp prop that stocks the hediff used for the psychic protection comp (it is also an extension of an existing comp)
        public HediffDef defaultHediff;
        public HediffDef alternatetHediff;

        public string defaultNameHediff = "FL_PsychichProtectionTank0";
        public string AlternateNameHediff = "FL_PsychichProtectionDodge0";

        public CompProperties_PsychicProtection() {
            compClass = typeof(Comp_PsychicProtection);
        }
    }
}
