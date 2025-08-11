using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    public class Comp_PsychicProtection : CompAbilityEffect_GiveHediff {
        //Extension comp to be able to apply the swtich ability
        public bool isAlternateMode = false;
        public CompProperties_PsychicProtection ProtectionProps => (CompProperties_PsychicProtection)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest) {
            Gene_PsychicProtection gene = getPsychicProtectionGene();
            string HediffName = "";
            if (isAlternateMode) {
                Log.Message($"Ability was alternate and pawn is {parent.pawn}");
                ProtectionProps.hediffDef = ProtectionProps.defaultHediff;
                HediffName = "FL_PsychichProtectionDodge";
            }
            else {
                Log.Message($"Ability was normal and pawn is {parent.pawn}");
                ProtectionProps.hediffDef = ProtectionProps.alternatetHediff;
                HediffName = "FL_PsychichProtectionTank";
            }
            isAlternateMode = !isAlternateMode;
            if (gene != null) {
                gene.isAlternateMode = isAlternateMode;
                HediffUtils.RemoveHediffWithString(HediffName, parent.pawn);
                gene.psychicSensitivityRecent = 0f;
                gene.channelingStageRecent = 0;
            }
            Log.Message($"Here is the gene: {gene}, {gene.isAlternateMode}, {isAlternateMode}");
            base.Apply(target, dest);
        }

        private Gene_PsychicProtection getPsychicProtectionGene() {
            Gene_PsychicProtection psychicProc = parent.pawn.genes.GetGene(InternalDefOf.FL_PsychicProtection) as Gene_PsychicProtection;
            if (psychicProc != null) {
                return psychicProc;
            }
            return null;
        }

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref isAlternateMode, "isAlternateMode", defaultValue: false);
        }
    }
}
