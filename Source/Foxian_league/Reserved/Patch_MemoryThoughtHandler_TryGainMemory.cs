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
    //Patch to make memories last much longer and have a bigger debuff for pawns with the "Reserved" gene
    [HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[] { typeof(Thought_Memory), typeof(Pawn) })]
    public static class Patch_MemoryThoughtHandler_TryGainMemory {
        [HarmonyPostfix]
        public static void Postfix(ref Thought_Memory newThought, Pawn otherPawn, MemoryThoughtHandler __instance) {
            if (newThought == null || !__instance.pawn.story.traits.HasTrait(InternalDefOf.FL_Reserved_Trait)) return;
            if (newThought.CurStage.baseMoodEffect < 0) {
                Log.Message($"Pawn has trait {__instance.pawn.story.traits.HasTrait(InternalDefOf.FL_Reserved_Trait)}");
                int totalMoodEffect = (int)Math.Round(newThought.CurStage.baseMoodEffect * newThought.moodPowerFactor);
                int offsetMood = (int)Math.Round(totalMoodEffect * 0.25);

                Log.Message(offsetMood);
                newThought.moodOffset = offsetMood;
                newThought.durationTicksOverride = Mathf.RoundToInt(newThought.DurationTicks * 1.5f);
                // moodpowerfactor seems to be a total percentage
            }
            return;
        }
    }
}
