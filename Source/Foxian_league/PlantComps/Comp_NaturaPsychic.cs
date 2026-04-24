using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using Verse;

namespace Foxian_league{
    public class Comp_NaturaPsychic : ThingComp {
        //Comp for the psychic tree and all the methods for it's functioning
        public CompProperties_NaturaPsychic Props => (CompProperties_NaturaPsychic)props;
        public float progressUntilNextBlessing;
        private float maxProgress = 450000f;
        public int meditationTickToday = 0;

        private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>> {
            new Pair<int, float>(25000, 1f),
            new Pair<int, float>(36000, 0.5f),
            new Pair<int, float>(50000, 0.25f),
            new Pair<int, float>(61000, 0.15f)
        };

        private float progressMultiplier {
            get {
                foreach (Pair<int, float> TicksToProgressMultiplier in TicksToProgressMultipliers) {
                    if (meditationTickToday < TicksToProgressMultiplier.First) {
                        return TicksToProgressMultiplier.Second;
                    }
                }
                return TicksToProgressMultipliers.Last().Second;
            }
        }

        public override void CompTickLong() {
            if(GenLocalDate.DayTick(parent.Map) < 2000) {
                meditationTickToday = 0;
            }
        }

        public override string CompInspectStringExtra() {
            return string.Concat("ProgressUntilNextBlessing".Translate((progressUntilNextBlessing/(maxProgress * Foxian_Settings.maxProgressMultiplier)).ToStringPercent()) + "\n" + "TotalMeditationTree".Translate((meditationTickToday/2500).ToString()) + " " + "ProgressMultiplierLoc".Translate(progressMultiplier.ToStringPercent()));

        }

        public void addProgress(float externalProgress = 0f) {
            if(externalProgress != 0f) progressUntilNextBlessing += externalProgress;
            progressUntilNextBlessing += (3f * progressMultiplier);
            meditationTickToday ++;
            tryTriggerBlessing();

        }

        private void tryTriggerBlessing() {
            if (progressUntilNextBlessing >= maxProgress * Foxian_Settings.maxProgressMultiplier) {
                progressUntilNextBlessing = 0f;
                List<Pawn> validPawns = getPawnNearTree();
                if (validPawns.Count == 0) return;

                List<Pawn> filteredPawns = filterPawns(validPawns);
                if(filteredPawns.Count == 0) return;

                Pawn chosenPawn = selectRandomPawn(filteredPawns);
                addHeddif(chosenPawn);
            }
        }

        public List<Pawn> getPawnNearTree() {
            float FocusObjectSearchRadius = MeditationUtility.FocusObjectSearchRadius;
            List<Pawn> validPawn = new List<Pawn>();
            foreach (Thing item in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, FocusObjectSearchRadius, useCenter: false)) {
                if (item is Pawn pawn && pawn.RaceProps.Humanlike && MeditationFocusTypeAvailabilityCache.PawnCanUse(pawn, MeditationFocusDefOf.Natural)) {
                    validPawn.Add(pawn);
                }
            }
            return validPawn;
        }

        public List<Pawn> filterPawns(List<Pawn> pawns) {
            List<Pawn> pawnsWithouthMaxedHediff = removeMaxedOutPawns(pawns);
            if(pawnsWithouthMaxedHediff.Count == 0) return pawnsWithouthMaxedHediff;

            else if(Rand.Chance(0.45f) && anyPawnsFreeOfHediff(pawnsWithouthMaxedHediff)) {
                //Focus pawns that does not have the hediff
                List<Pawn> pawnsWithoutHediff = new List<Pawn>();
                foreach(Pawn pawn in pawnsWithouthMaxedHediff) {
                    if(pawn == null) continue;
                    if(!pawn.health.hediffSet.HasHediff(InternalDefOf.FL_Tree_Connection)) {
                        pawnsWithoutHediff.Add(pawn);
                    }
                }
                return pawnsWithoutHediff;
            }
            return pawnsWithouthMaxedHediff;
        }

        private List<Pawn> removeMaxedOutPawns(List<Pawn> pawns) {
            List<Pawn> pawnsWithoutMaxedHediff = new List<Pawn>();
            Hediff hediffToCheck;
            foreach(Pawn pawn in pawns) {
                if(pawn.health.hediffSet.TryGetHediff(InternalDefOf.FL_Tree_Connection, out hediffToCheck) && hediffToCheck.Severity >= 1f) {
                    continue;
                }
                pawnsWithoutMaxedHediff.Add(pawn);
            }
            return pawnsWithoutMaxedHediff;
        }

        private bool anyPawnsFreeOfHediff(List<Pawn> pawns) {
            foreach(Pawn pawn in pawns) {
                if(!pawn.health.hediffSet.HasHediff(InternalDefOf.FL_Tree_Connection)) {
                    return true;
                }
            }
            return false;
        }

        public Pawn selectRandomPawn(List<Pawn> pawns) {
            int totalPawns = pawns.Count;
            if(totalPawns == 1) return pawns[0];

            float randArray = (Rand.Value * ((totalPawns) - 1f) + 1f);
            decimal roundedRandArray = Math.Round((decimal)randArray);
            int selectArray = Math.Clamp((int)roundedRandArray, 0, totalPawns) - 1;
            return pawns[selectArray];
        }

        public void addHeddif(Pawn pawn) {
            Hediff hediffToIncrease;
            if(pawn.health.hediffSet.TryGetHediff(InternalDefOf.FL_Tree_Connection, out hediffToIncrease)) {
                if(!(hediffToIncrease.Severity >= 1f)) {
                    hediffToIncrease.Severity += 0.1f;
                }
            }
            else {
                Hediff treeConnection = HediffMaker.MakeHediff(InternalDefOf.FL_Tree_Connection, pawn);
                treeConnection.Severity = 0.1f;
                pawn.health.AddHediff(treeConnection);
            }
            ChoiceLetter_BlessingReceived choiceLetter_blessing = (ChoiceLetter_BlessingReceived)LetterMaker.MakeLetter("BlessingReceivedTitle".Translate(pawn), "BlessingReceivedTitleLoc".Translate(pawn), InternalDefOf.FL_BlessingReceived, pawn);
            choiceLetter_blessing.Start();
            Find.LetterStack.ReceiveLetter(choiceLetter_blessing);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra() {
            if(Prefs.DevMode) {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEV: Add 100% progress";
                command_Action.action = delegate
                {
                    addProgress(maxProgress * Foxian_Settings.maxProgressMultiplier);
                };
                yield return command_Action;
            }
        }

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref progressUntilNextBlessing, "progressUntilNextBlessing", defaultValue: 0f);
            Scribe_Values.Look(ref meditationTickToday, "meditationTickToday", defaultValue: 0);
        }
    }
}
