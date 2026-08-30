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
            Gene_PsychicProtectionDynamic gene = getPsychicProtectionGene();
            string hediffName;
            if (isAlternateMode) {
                ProtectionProps.hediffDef = ProtectionProps.defaultHediff;
                hediffName = ProtectionProps.alternatetHediff.defName;
            }
            else {
                ProtectionProps.hediffDef = ProtectionProps.alternatetHediff;
                hediffName = ProtectionProps.defaultHediff.defName;
            }
            isAlternateMode = !isAlternateMode;
            if (gene != null) {
                gene.isAlternateMode = isAlternateMode;
                HediffUtils.RemoveHediffWithString(hediffName, parent.pawn);
            }
            base.Apply(target, dest);
        }

        private Gene_PsychicProtectionDynamic getPsychicProtectionGene() {
            Gene_PsychicProtectionDynamic psychicProc = parent.pawn.genes.GetGene(InternalDefOf.FL_PsychicProtection) as Gene_PsychicProtectionDynamic;
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
