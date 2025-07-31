using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace Foxian_league {
    //Gene class to dynamically change the stage of the hediff for a pawn with the gene
    public class Gene_PsychicChanneling : Gene {
        public float psychicSensitivityRecent;
        public int channelingStageRecent = 0;

        private const float channelingStage1 = 1.25f;
        private const float channelingStage2 = 1.5f;
        private const float channelingStage3 = 1.75f;
        private const float channelingStage4 = 2f;
        private const float channelingStage5 = 2.5f;

        private const string hediffName = "FL_PsychichChanneling";

        public override void TickInterval(int delta) {
            base.TickInterval(delta);
            float PsychichSensiPawn = MathF.Round(pawn.GetStatValue(StatDefOf.PsychicSensitivity), 2);
            if (psychicSensitivityRecent != PsychichSensiPawn) {
                psychicSensitivityRecent = PsychichSensiPawn;
                int currentChannelingStage = GetChannelingStage(PsychichSensiPawn);
                Log.Message($"recent psychic sens: {psychicSensitivityRecent} and recent ChannState: {channelingStageRecent}");
                Log.Message($"current psychic sens: {PsychichSensiPawn} and current ChannState: {currentChannelingStage}");

                if (currentChannelingStage != channelingStageRecent) {
                    Log.Message("current channel is not the same as recent one");
                    RemoveChannelingStage(hediffName, channelingStageRecent);
                    channelingStageRecent = currentChannelingStage;
                    SetChannelingStage(hediffName, currentChannelingStage);
                }
            }
        }
        public override void PostAdd() {
            base.PostAdd();
            float PsychichSensiPawn = pawn.GetStatValue(StatDefOf.PsychicSensitivity);
            psychicSensitivityRecent = PsychichSensiPawn;
            int currentChannelingStage = GetChannelingStage(PsychichSensiPawn);
            channelingStageRecent = currentChannelingStage;
            SetChannelingStage(hediffName, currentChannelingStage);
        }

        public override void PostRemove() {
            base.PostRemove();
            RemoveChannelingStage(hediffName, channelingStageRecent);
        }

        public int GetChannelingStage(float currentPawnPsySensitivity) {
            return currentPawnPsySensitivity switch {
                >= channelingStage1 and < channelingStage2 => 1,
                >= channelingStage2 and < channelingStage3 => 2,
                >= channelingStage3 and < channelingStage4 => 3,
                >= channelingStage4 and < channelingStage5 => 4,
                >= channelingStage5 => 5,
                _ => 0,
            };
        }

        public void SetChannelingStage(string hediffName, int stage) {
            if(pawn != null) {
                HediffDef hediffDef = HediffDef.Named($"{hediffName}{stage}");
                hediffDef.initialSeverity = float.Epsilon;
                pawn.health.AddHediff(hediffDef);
            }
            return;
        }

        public void RemoveChannelingStage(string hediffName, int stage) {
            if (pawn != null) {
                Hediff hediffToRemove = pawn.health.hediffSet.hediffs.Find(x => x.def.defName == $"{hediffName}{stage}");
                if (hediffToRemove != null) {
                    pawn.health.RemoveHediff(hediffToRemove);
                }
                return;
            }
            return;
        }
    }
}
