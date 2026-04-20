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
        //Comp for the Natura tree to work
        public CompProperties_NaturaPsychic Props => (CompProperties_NaturaPsychic)props;
        public float progressUntilNextBlessing;
        private float maxProgress = 480000f;
        public int meditationTickToday = 0;

        private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>> {
            new Pair<int, float>(22500, 1f),
            new Pair<int, float>(35000, 0.5f),
            new Pair<int, float>(50000, 0.25f),
            new Pair<int, float>(60000, 0.15f)
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
                Log.Message("TreeComp is active and it's day time OWO");
                meditationTickToday = 0;
            }
        }

        public override string CompInspectStringExtra() {
            return string.Concat("ProgressUntilNextBlessing".Translate((progressUntilNextBlessing/(maxProgress * Foxian_Settings.maxProgressMultiplier)).ToStringPercent()) + "\n" + "TotalMeditationTree".Translate((meditationTickToday/2500).ToString()) + " " + "ProgressMultiplierLoc".Translate(progressMultiplier.ToStringPercent()));

        }

        public void addProgress() {
            progressUntilNextBlessing += (3f * progressMultiplier);
            meditationTickToday ++;
            Log.Message($"Meditation tick ++ {meditationTickToday}, progress mult with malus: {3f*progressMultiplier}, maxprogress is {maxProgress * Foxian_Settings.maxProgressMultiplier}");
            tryTriggerBlessing();

        }

        private void tryTriggerBlessing() {
            if (progressUntilNextBlessing >= maxProgress * Foxian_Settings.maxProgressMultiplier) {
                checkPawnNearTree();
                progressUntilNextBlessing = 0f;
            }
        }

        public void checkPawnNear (Pawn pawn) {
            if(pawn == null) return;
            MeditationSpotAndFocus spot = MeditationUtility.FindMeditationSpot(pawn);
            Log.Message($"Pawn is: {pawn} and spot is: {spot.focus} ({spot.focus.Cell}) and {spot.spot} ({spot.spot.Cell}), here is parent spot as comp: {parent.Position}");
        }

        public void checkPawnNearTree() {
            float FocusObjectSearchRadius = MeditationUtility.FocusObjectSearchRadius;
            List<Pawn> validPawn = new List<Pawn>();
            foreach (Thing item in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, FocusObjectSearchRadius, useCenter: false)) {
                if (item is Pawn pawn && Utils.HasActiveGene(pawn, InternalDefOf.FL_NaturalPsySensitive)) {
                    validPawn.Add(pawn);
                    Log.Message($"Found pawn {pawn} near the tree");
                }
            }
            Log.Message($"Total valid pawns found near the tree: {validPawn.Count} and {validPawn}");
        }

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref progressUntilNextBlessing, "progressUntilNextBlessing", defaultValue: 0f);
            Scribe_Values.Look(ref meditationTickToday, "meditationTickToday", defaultValue: 0);
        }
    }
}
