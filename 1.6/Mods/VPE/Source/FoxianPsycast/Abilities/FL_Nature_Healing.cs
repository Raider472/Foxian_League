using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class FL_Nature_Healing : VEF.Abilities.Ability {
        public override void Cast(params GlobalTargetInfo[] targets) {
            base.Cast(targets);
            float psychicSensitivityMin = getPsychicSensitivityMin();
            Log.Message($"Psychic Sensitivity Min: {psychicSensitivityMin}");
            if(targets.Length == 0 || targets == null) return;
            for(int i = 0; i < targets.Length; i++) {
                Log.Message($"{targets[i].Pawn}");
                List<Hediff_Injury> injuriesList = targets[i].Pawn.health.hediffSet.hediffs.OfType<Hediff_Injury>().ToList();
                Log.Message($"Injuries found: {injuriesList.Count}");
                if(injuriesList.Any()) {
                    HealInjuries(injuriesList, psychicSensitivityMin);
                }
                else {
                    Log.Message("No injuries to heal");
                    continue;
                }
            }
        }

        private void HealInjuries(List<Hediff_Injury> injuries, float psychicSensitivityMin) {
            foreach(Hediff_Injury injury in injuries) {
                if (injury.IsPermanent()) continue;
                float skipHeal = Rand.Value;
                if(skipHeal < 0.2f) {
                    Log.Message($"Skipping healing for {injury.Label} with severity {injury.Severity}");
                    continue;
                }
                Log.Message($"{injury.Label} and severity {injury.Severity}");
                float randomHealValue = (Rand.Value * (injury.Severity - psychicSensitivityMin) + psychicSensitivityMin);
                injury.Heal(randomHealValue);
                Log.Message($"Healed {randomHealValue} point of {injury.Label}, new severity: {injury.Severity}");
            }
        }

        private float getPsychicSensitivityMin() {
            return pawn.psychicEntropy.PsychicSensitivity * 0.1f;
        }
    }
}
