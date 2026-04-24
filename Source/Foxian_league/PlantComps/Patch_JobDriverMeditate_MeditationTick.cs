using HarmonyLib;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Foxian_league {

    [HarmonyPatch(typeof(JobDriver_Meditate), "MeditationTick")]
    public class Patch_JobDriverMeditate_MeditationTick {
        //Patch to get the Natura tree to progress when a pawn meditates near it
        private static void activateTree(Pawn pawn) {
            int num = GenRadial.NumCellsInRadius(MeditationUtility.FocusObjectSearchRadius);
            for(int i = 0; i < num; i++) {
                IntVec3 c = pawn.Position + GenRadial.RadialPattern[i];
                if(c.InBounds(pawn.Map)) {
                    Plant plant = c.GetPlant(pawn.Map);
                    if(plant != null && plant.def == InternalDefOf.FL_Plant_TreeNatura && plant.LifeStage == PlantLifeStage.Mature) {
                        plant.TryGetComp<Comp_NaturaPsychic>()?.addProgress();
                    }
                }
            }
        }

        [HarmonyPostfix]
        public static void Postfix(JobDriver_Meditate __instance) {
            Pawn pawn = __instance.pawn;
            if(MeditationFocusTypeAvailabilityCache.PawnCanUse(pawn, MeditationFocusDefOf.Natural)) {
                activateTree(pawn);
            }
            return;
        }
    }
}
