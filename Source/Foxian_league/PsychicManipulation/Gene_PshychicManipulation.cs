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
        public float negotiationAbility;
        public float tradePriceImprovement;
        public float psychicSensitivityRecent;

        public float delimiter = Foxian_Settings.psychicManipulationFactor;

        public override void TickInterval(int delta) {
            base.TickInterval(delta);
            if (!pawn.IsHashIntervalTick(200)) {
                return;
            }
            //Log.Message($"[Foxian League] TickInterval called for {pawn.Name} with TradeNegotiation and TradePriceImprovement: {pawn.GetStatValue(StatDefOf.NegotiationAbility)}, {pawn.GetStatValue(StatDefOf.TradePriceImprovement)}");
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
                skillModifier = Math.Clamp((int)roundedSkillModifier, 0, 20);
                negotiationAbility = skillModifier * 0.05f;
                tradePriceImprovement = skillModifier * 0.01f;
            }
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref skillModifier, "skillModifier", defaultValue: 0);
            Scribe_Values.Look(ref psychicSensitivityRecent, "psychicSensitivityRecent", defaultValue: 0);
            Scribe_Values.Look(ref negotiationAbility, "negotiationAbility", defaultValue: 0);
            Scribe_Values.Look(ref tradePriceImprovement, "tradePriceImprovement", defaultValue: 0);
        }
    }
}
