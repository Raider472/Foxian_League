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

        private const float channelingStage1 = 1.25f;
        private const float channelingStage2 = 1.5f;
        private const float channelingStage3 = 1.75f;
        private const float channelingStage4 = 2f;
        private const float channelingStage5 = 2.5f;

        private const string hediffName = "";

        public int GetChannelingStage(float currentPawnPsySensitivity) {
            return currentPawnPsySensitivity switch {
                >= channelingStage1 and < channelingStage2 => 1,
                >= channelingStage2 and < channelingStage3 => 2,
                >= channelingStage3 and < channelingStage4 => 3,
                >= channelingStage4 and < channelingStage5 => 4,
                >= channelingStage5 => 5,
                _ => 0,
            };
        }
    }
}
