using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace Foxian_league {
    //Gene class to dynamically change the stage of the hediff for a pawn with the gene
    public class Gene_PsychicChanneling : Gene_PsychicStage {

        private const string hediffName = "FL_PsychichChanneling";

        public override void TickInterval(int delta) {
            //base.TickInterval(delta);
            if (!pawn.IsHashIntervalTick(150)) {
                //Log.Message($"{tickInterval}");
                return;
            }
            float PsychichSensiPawn = MathF.Round(pawn.GetStatValue(StatDefOf.PsychicSensitivity), 2);
            if (psychicSensitivityRecent == PsychichSensiPawn) return;
            psychicSensitivityRecent = PsychichSensiPawn;
            int currentChannelingStage = GetChannelingStage(PsychichSensiPawn);
            Log.Message($"recent psychic sens: {psychicSensitivityRecent} and recent ChannState: {channelingStageRecent}");
            Log.Message($"current psychic sens: {PsychichSensiPawn} and current ChannState: {currentChannelingStage}");

            if (currentChannelingStage == channelingStageRecent) return;
            Log.Message("current channel is not the same as recent one");
            HediffUtils.RemoveHediffStage(hediffName, channelingStageRecent, pawn);
            channelingStageRecent = currentChannelingStage;
            HediffUtils.SetHediffStage(hediffName, currentChannelingStage, pawn);
        }

        public override int GetChannelingStage(float currentPawnPsySensitivity) {
            return currentPawnPsySensitivity switch {
                >= channelingStage1 and < channelingStage2 => 1,
                >= channelingStage2 and < channelingStage3 => 2,
                >= channelingStage3 and < channelingStage4 => 3,
                >= channelingStage4 and < channelingStage5 => 4,
                >= channelingStage5 => 5,
                _ => 0,
            };
        }

        public override void PostAdd() {
            base.PostAdd();
            Log.Message("Test gene has been added");
            float PsychichSensiPawn = MathF.Round(pawn.GetStatValue(StatDefOf.PsychicSensitivity), 2);
            psychicSensitivityRecent = PsychichSensiPawn;
            int currentChannelingStage = GetChannelingStage(PsychichSensiPawn);
            channelingStageRecent = currentChannelingStage;
            HediffUtils.SetHediffStage(hediffName, currentChannelingStage, pawn);
        }

        public override void PostRemove() {
            base.PostRemove();
            HediffUtils.RemoveHediffStage(hediffName, channelingStageRecent, pawn);
        }
    }
}
