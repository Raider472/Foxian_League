using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class RitualOutcomeEffectWorker_SacrificialTree: RitualOutcomeEffectWorker_FromQuality {
        public static readonly SimpleCurve PercentageFromQuality = new SimpleCurve {
            new CurvePoint(0.2f, 0f),
            new CurvePoint(0.4f, 0.2f),
            new CurvePoint(0.6f, 0.4f),
            new CurvePoint(0.8f, 0.5f),
            new CurvePoint(1f, 0.6f)
        };

        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_SacrificialTree() {
        }

        public RitualOutcomeEffectWorker_SacrificialTree(RitualOutcomeEffectDef def)
            : base(def) {
        }

        //TODO Add an outcome after the ritual is complete
        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual) {
            float quality = GetQuality(jobRitual, progress);
            Pawn pawn = jobRitual.PawnWithRole("organizer");
            int num = (int)PercentageFromQuality.Evaluate(quality);
            string text = "LetterTextSacrificialRitualCompleted".Translate(pawn);
            Log.Message($"Sacrificial Tree Ritual Completed: {num} Quality.");
            text = text + "\n\n" + OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            Find.LetterStack.ReceiveLetter("LetterLabelSacrificialRitualCompleted".Translate(), text, LetterDefOf.RitualOutcomePositive, new LookTargets(pawn, jobRitual.selectedTarget.Thing));
        }
    }
}
