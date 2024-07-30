using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [HarmonyPatch(typeof(Pawn_GeneTracker), "Notify_GenesChanged")]
    public static class FixGender {
        [HarmonyPostfix]
        public static void Postfix(ref Pawn ___pawn, GeneDef addedOrRemovedGene) {
            bool flag = false;
            if (addedOrRemovedGene == InternalDefOf.FL_Female && ___pawn.gender != Gender.Female) {
                ___pawn.gender = Gender.Female;
                if (___pawn.story.bodyType == BodyTypeDefOf.Male) {
                    ___pawn.story.bodyType = BodyTypeDefOf.Female;
                }
                flag = true;
            }
            if (flag) {
                if (___pawn.story.headType.gender != 0 && ___pawn.story.headType.gender != ___pawn.gender && !___pawn.story.TryGetRandomHeadFromSet(DefDatabase<HeadTypeDef>.AllDefs.Where((HeadTypeDef x) => x.randomChosen))) {
                    Log.Warning("Couldn't find an appropriate head after changing pawn gender.");
                }
                if (!___pawn.style.CanWantBeard && ___pawn.style.beardDef != BeardDefOf.NoBeard) {
                    ___pawn.style.beardDef = BeardDefOf.NoBeard;
                }
                ___pawn.Drawer.renderer.SetAllGraphicsDirty();
            }
        }
    }
}
