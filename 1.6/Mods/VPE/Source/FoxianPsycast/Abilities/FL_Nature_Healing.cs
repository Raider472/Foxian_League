using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FoxianPsycast {
    public class FL_Nature_Healing : VEF.Abilities.Ability {
        private float chanceOfSkip = 0f;
        public static readonly SimpleCurve MinMendingQualityFromPsychicSensitivity = new SimpleCurve {
            new CurvePoint(0.5f, 0f),
            new CurvePoint(1f, 0.0f),
            new CurvePoint(1.5f, 0.1f),
            new CurvePoint(2f, 0.2f),
            new CurvePoint(2.5f, 0.4f),
            new CurvePoint(3f, 0.5f),
            new CurvePoint(4f, 0.7f),
            new CurvePoint(5f, 1f),
            new CurvePoint(10f, 2f)
        };
        public static readonly SimpleCurve MaxMendingQualityFromPsychicSensitivity = new SimpleCurve {
            new CurvePoint(0.5f, 0.7f),
            new CurvePoint(1f, 0.8f),
            new CurvePoint(1.5f, 0.85f),
            new CurvePoint(2f, 1f),
            new CurvePoint(2.5f, 1f),
            new CurvePoint(3f, 1.5f),
            new CurvePoint(4f, 1.8f),
            new CurvePoint(5f, 2f),
            new CurvePoint(10f, 2f)
        };
        public override void Cast(params GlobalTargetInfo[] targets) {
            base.Cast(targets);
            float psychicSensitivityMin = getPsychicSensitivityMin();
            Log.Message($"Psychic Sensitivity Min: {psychicSensitivityMin}");
            if(targets.Length == 0 || targets == null) return;
            for(int i = 0; i < targets.Length; i++) {
                Log.Message($"{targets[i].Pawn}");
                List<Hediff_Injury> injuriesList = targets[i].Pawn.health.hediffSet.hediffs.OfType<Hediff_Injury>().Where(injury => !injury.IsPermanent()).ToList();
                Log.Message($"Injuries found: {injuriesList.Count}");
                if(injuriesList.Any()) {
                    HealInjuries(injuriesList, psychicSensitivityMin);
                    Messages.Message("NatureHealingMessage".Translate(injuriesList.Count.ToString(), targets[i].Pawn), ((LocalTargetInfo)targets[i]).ToTargetInfo(targets[i].Pawn.Map), MessageTypeDefOf.PositiveEvent, historical: false);
                }
                else {
                    Log.Message("No injuries to heal");
                    continue;
                }
            }
        }

        private void HealInjuries(List<Hediff_Injury> injuries, float psychicSensitivityMin) {
            int injuriesCount = 0;
            foreach(Hediff_Injury injury in injuries) {
                if (injury.IsPermanent()) continue;
                if(chanceOfSkip == 0f) chanceOfSkip = Rand.Value;
                else chanceOfSkip += 0.15f;

                Log.Message($"Chance of skip: {chanceOfSkip}");
                if(chanceOfSkip > 0.65f && injuriesCount != 0) {
                    Log.Message($"Skipping healing for {injury.Label} with severity {injury.Severity}");
                    chanceOfSkip = 0f;
                    continue;
                }
                Log.Message($"{injury.Label} and severity {injury.Severity}");
                float randomHealValue = (Rand.Value * (injury.Severity - psychicSensitivityMin) + psychicSensitivityMin);
                float randTendQuality = Rand.Range(getTendQualityMin(), getTendQualityMax());
                Log.Message($"min: {getTendQualityMin()}, max: {getTendQualityMax()}");
                Log.Message($"Random heal value: {randomHealValue}, Tend quality: {randTendQuality}");

                injury.Heal(randomHealValue);
                if(!injury.IsTended()) {
                    Log.Message($"Injury {injury.Label} is not tended.");
                    injury.Tended(randTendQuality, getTendQualityMax());
                }
                Log.Message($"Healed {randomHealValue} point of {injury.Label}, new severity: {injury.Severity}");
                injuriesCount++;
            }
        }

        private float getPsychicSensitivityMin() {
            return pawn.psychicEntropy.PsychicSensitivity * 0.1f;
        }

        private float getTendQualityMin() {
            return MinMendingQualityFromPsychicSensitivity.Evaluate(pawn.psychicEntropy.PsychicSensitivity);
        }

        private float getTendQualityMax() {
            return MaxMendingQualityFromPsychicSensitivity.Evaluate(pawn.psychicEntropy.PsychicSensitivity);
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref chanceOfSkip, "chanceOfSkip", 0f);
        }
    }
}
