using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league{
    //Current Gene class used to dynamically calculate the bonus based on psychic sensitivity of the pawn with an alt bool
    public class Gene_PsychicProtectionDynamic: Gene {
        public bool isAlternateMode = false;

        private const string hediffName = "FL_PsychichProtectionTank";
        private const string hediffNameAlternate = "FL_PsychichProtectionDodge";

        public override void PostAdd() {
            base.PostAdd();
            HediffUtils.AddHediffWithString(hediffName, pawn);
        }

        public override void PostRemove() {
            base.PostRemove();
            if(isAlternateMode) {
                HediffUtils.RemoveHediffWithString(hediffNameAlternate, pawn);
            }
            else {
                HediffUtils.RemoveHediffWithString(hediffName, pawn);
            }
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref isAlternateMode, "isAlternateMode", defaultValue: false);
        }
    }
}
