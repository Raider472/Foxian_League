using FoxianPsycast;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace FoxianPsycast {
    public class RitualBehaviorWorker_SacrificialTree: RitualBehaviorWorker {
        public RitualBehaviorWorker_SacrificialTree() {
        }

        public RitualBehaviorWorker_SacrificialTree(RitualBehaviorDef def)
            : base(def) {
        }

        public override string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality) {
            int count = assignments.SpectatorsForReading.Count;
            float num = RitualOutcomeEffectWorker_SacrificialTree.PercentageFromQuality.Evaluate(quality);
            TaggedString taggedString = "SacrificialRitualExplanationBase".Translate((num + 0.4f).ToStringPercent());
            return taggedString;
        }

        public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality) {
            int count = assignments.SpectatorsForReading.Count;
            return Mathf.RoundToInt((float)ritual.behavior.def.durationTicks.max / RitualStage_SacrificialTree.ProgressPerParticipantCurve.Evaluate(count + 1)).ToStringTicksToPeriod(allowSeconds: false);
        }
    }
}
