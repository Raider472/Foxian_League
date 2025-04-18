using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {

    [HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[] { typeof(Thought_Memory), typeof(Pawn) })]
    public static class HarmonyPatchTest {
        [HarmonyPostfix]
        public static void Postfix(ref Thought_Memory newThought, Pawn otherPawn, MemoryThoughtHandler __instance) {
            Log.Message($"new thoguht base mood {newThought.moodOffset} for {newThought}");
            if (__instance.pawn.genes.HasActiveGene(InternalDefOf.FL_Reserved)) {
                Log.Message($"Pawn {__instance.pawn} has the gene");
                Log.Message($"Other Pawn {newThought.otherPawn}");
                Log.Message($"Though durationTick {newThought.DurationTicks}");
                Log.Message($"Though durationTickOveride {newThought.durationTicksOverride}");
                Log.Message($"Thought moodofset {newThought.moodOffset}");
                Log.Message($"Thoug mood power factor {newThought.moodPowerFactor}");
                Log.Message($"Thought is visible in need {newThought.VisibleInNeedsTab}");
                Log.Message($"Index of cur stage thought: {newThought.CurStageIndex}");
                Log.Message($"Thought List 0 index {newThought.def.stages[0].label}, {newThought.def.stages[0].baseMoodEffect}, {newThought.def.stages[0].baseOpinionOffset}");
                Log.Message($"Thought Size {newThought.def.stages.Count}");
                Log.Message($"Current Thought index {newThought.CurStage.label}, {newThought.CurStage.baseMoodEffect}, {newThought.CurStage.baseOpinionOffset}");
                Log.Message($"CachedMoodOffesetOfGroup {newThought.cachedMoodOffsetOfGroup}");
                Log.Message($"Current thoughtWorker {newThought.def.Worker}");
                Log.Message($"Current defstatseffect {newThought.def.effectMultiplyingStat}");
                newThought.durationTicksOverride = Mathf.RoundToInt(newThought.DurationTicks * 1.5f);
                // moodpowerfactor seems to be a total percentage
            }
        }
    }
}
