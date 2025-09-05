using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Class gene to handle the dynamic psychic calculation, the bonus attribute is stored in the skill modifier field
    public class Gene_PsychicManipulation : Gene {

        public int skillModifier;
        public float psychicSensitivityRecent;
        public int tickInterval = 0;

        public float delimiter = 0.25f;

        public override void TickInterval(int delta) {
            base.TickInterval(delta);
            if (tickInterval < 150) {
                tickInterval++;
                Log.Message($"{tickInterval}");
                return;
            }
            tickInterval = 0;
            float PsychichSensiPawn = pawn.GetStatValue(StatDefOf.PsychicSensitivity);
            if (PsychichSensiPawn == psychicSensitivityRecent) return;
            psychicSensitivityRecent = PsychichSensiPawn;
            CalculateSkillModifier(psychicSensitivityRecent);
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
            }
        }
    }
}
