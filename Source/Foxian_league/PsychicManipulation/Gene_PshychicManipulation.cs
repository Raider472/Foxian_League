using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Gene_PsychicManipulation : Gene {

        public int skillModifier;
        public float psychicSensitivityRecent;

        public float delimiter = 0.5f;

        public override void Tick() {
            base.Tick();
            float PsychichSensiPawn = pawn.psychicEntropy.PsychicSensitivity;
            Log.Message($"PsychichSensiPawn {PsychichSensiPawn}, {pawn.Name}, {skillModifier}, {psychicSensitivityRecent}");
            if (psychicSensitivityRecent != PsychichSensiPawn) {
                Log.Message("Psychic has changed");
                Log.Message($"skill before {skillModifier}");
                Log.Message($"psychicSensitivityRecent before change {psychicSensitivityRecent}");
                psychicSensitivityRecent = PsychichSensiPawn;
                Log.Message($"psychicSensitivityRecent after change {psychicSensitivityRecent}");
                CalculateSkillModifier(psychicSensitivityRecent);
            }
        }

        private void CalculateSkillModifier(float currentPawnPsySensitivity) {
            if (currentPawnPsySensitivity <= 1) {
                skillModifier = 0;
            }
            else {
                currentPawnPsySensitivity -= 1f;
                float skillModifierFloat = currentPawnPsySensitivity / delimiter;
                decimal roundedSkillModifier= Math.Round((decimal)skillModifierFloat, 1);
                skillModifier = (int)roundedSkillModifier;
                Log.Message($"skill after {skillModifier}");
            }
        }
    }
}
