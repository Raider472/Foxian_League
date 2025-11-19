using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Abstract class so I can avoid creating the same fields
    public abstract class Gene_PsychicStage : Gene {
        public float psychicSensitivityRecent;
        public int channelingStageRecent = 0;

        public const float channelingStage1 = 1.25f;
        public const float channelingStage2 = 1.5f;
        public const float channelingStage3 = 1.75f;
        public const float channelingStage4 = 2f;
        public const float channelingStage5 = 2.5f;
        public const float channelingStage6 = 3f;

        private const string hediffName = "";

        public virtual int GetChannelingStage(float currentPawnPsySensitivity) {
            return 0;
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref channelingStageRecent, "channelingStageRecent", defaultValue: 0);
            Scribe_Values.Look(ref psychicSensitivityRecent, "psychicSensitivityRecent", defaultValue: 0);
        }
    }
}
