using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Current Gene class used to dynamically calculate the bonus based on psychic sensitivity of the pawn
    public class Gene_PsychicChannelingDynamic : Gene {

        private const string hediffName = "FL_PsychichChanneling";

        public override void PostAdd() {
            base.PostAdd();
            HediffUtils.AddHediffWithString(hediffName, pawn);
        }

        public override void PostRemove() {
            base.PostRemove();
            HediffUtils.RemoveHediffWithString(hediffName, pawn);
        }
    }
}
