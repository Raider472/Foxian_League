﻿using RimWorld;
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
            if (tickInterval < 150) {
                tickInterval++;
                //Log.Message($"{tickInterval}");
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
