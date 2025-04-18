using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Gene_PsychicManipulation : Gene {

        public int skillModifier = 0;
        public float PsychicSensitivityRecent;

        public Aptitude AptitudeSkill;

        public override void Tick() {
            base.Tick();
            float PsychichSensiPawn = pawn.psychicEntropy.PsychicSensitivity;
            /*Log.Message($"Current saved Psychic Sensivity: {PsychicSensitivityRecent}");
            Log.Message($"Current saved Skill MOdifier: {skillModifier}");
            Log.Message($"Psychic Sensitivity owo: {PsychichSensiPawn}");*/
            if (PsychicSensitivityRecent != PsychichSensiPawn) {
                //Log.Message("Psychic Sens is not the same");
                PsychicSensitivityRecent = PsychichSensiPawn;
                CalculateSkillModifier(PsychicSensitivityRecent);
            }
        }

        private void CalculateSkillModifier(float currentPawnPsySensitivity) {
            if (currentPawnPsySensitivity <= 1) {
                skillModifier = 0;
            }
            else {
                currentPawnPsySensitivity -= 1f;
                float skillModifierFloat = currentPawnPsySensitivity / 0.5f;
                decimal roundedSkillModifier= Math.Round((decimal)skillModifierFloat, 1);
                skillModifier = (int)roundedSkillModifier;
                CreateAptitude();
                UpdateSkillLevelBonus();
            }
        }

        private void CreateAptitude() {
            SkillDef skillSocial = DefDatabase<SkillDef>.GetNamed("Social");
            AptitudeSkill = new Aptitude(skillSocial, skillModifier);
        }

        private void UpdateSkillLevelBonus() {
            if (!def.aptitudes.NullOrEmpty()) {
                def.aptitudes.Clear();
            }
            List<Aptitude> aptitudesList = [AptitudeSkill];
            def.aptitudes = aptitudesList;
        }
    }
}
