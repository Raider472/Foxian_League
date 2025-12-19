using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Gene_PsychicProtection : Gene_PsychicStage {
        //Gene class to calculate the stage of the hediff, check in the comp if alternative mode has been activated or not
        public bool isAlternateMode = false;

        private const string hediffName = "FL_PsychichProtectionTank";
        private const string hediffNameAlternate = "FL_PsychichProtectionDodge";

        public override void TickInterval(int delta) {
            base.TickInterval(delta);
            if (!pawn.IsHashIntervalTick(150)) {
                return;
            }
            float PsychichSensiPawn = MathF.Round(pawn.GetStatValue(StatDefOf.PsychicSensitivity), 2);
            if (psychicSensitivityRecent == PsychichSensiPawn) return;
            psychicSensitivityRecent = PsychichSensiPawn;
            int currentChannelingStage = GetChannelingStage(PsychichSensiPawn);

            if (currentChannelingStage == channelingStageRecent) return;
            if (isAlternateMode) {
                HediffUtils.RemoveHediffStage(hediffNameAlternate, channelingStageRecent, pawn);
                channelingStageRecent = currentChannelingStage;
                HediffUtils.SetHediffStage(hediffNameAlternate, currentChannelingStage, pawn);
            }
            else {
                HediffUtils.RemoveHediffStage(hediffName, channelingStageRecent, pawn);
                channelingStageRecent = currentChannelingStage;
                HediffUtils.SetHediffStage(hediffName, currentChannelingStage, pawn);
            }
        }

        public override int GetChannelingStage(float currentPawnPsySensitivity) {
            return currentPawnPsySensitivity switch {
                >= channelingStage1 and < channelingStage2 => 1,
                >= channelingStage2 and < channelingStage3 => 2,
                >= channelingStage3 and < channelingStage4 => 3,
                >= channelingStage4 and < channelingStage5 => 4,
                >= channelingStage5 and < channelingStage6 => 5,
                >= channelingStage6 => 6,
                _ => 0,
            };
        }
        public override void PostAdd() {
            base.PostAdd();
            float PsychichSensiPawn = MathF.Round(pawn.GetStatValue(StatDefOf.PsychicSensitivity), 2);
            psychicSensitivityRecent = PsychichSensiPawn;
            int currentChannelingStage = GetChannelingStage(PsychichSensiPawn);
            channelingStageRecent = currentChannelingStage;
            HediffUtils.SetHediffStage(hediffName, currentChannelingStage, pawn);
        }

        public override void PostRemove() {
            base.PostRemove();
            if (isAlternateMode) {
                HediffUtils.RemoveHediffStage(hediffNameAlternate, channelingStageRecent, pawn);
            }
            else {
                HediffUtils.RemoveHediffStage(hediffName, channelingStageRecent, pawn);
            }
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref isAlternateMode, "isAlternateMode", defaultValue: false);
        }
    }
}
