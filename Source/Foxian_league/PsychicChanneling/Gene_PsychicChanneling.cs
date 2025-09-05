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
            if (tickInterval < 150) {
                tickInterval++;
                Log.Message($"{tickInterval}");
                return;
            }
            tickInterval = 0;
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
