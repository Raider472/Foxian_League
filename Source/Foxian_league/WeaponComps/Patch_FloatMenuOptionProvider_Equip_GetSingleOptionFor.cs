using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Foxian_league {
    //Create a confirmation Window asking if you really want to equip this item (cursed blade)
    [HarmonyPatch(typeof(FloatMenuOptionProvider_Equip), "GetSingleOptionFor")]
    public static class Patch_FloatMenuOptionProvider_Equip_GetSingleOptionFor {
        [HarmonyPostfix]
        public static void Postfix(ref FloatMenuOption __result, Thing clickedThing, FloatMenuContext context) {
            if (clickedThing == null || !clickedThing.HasComp<CompEquippable>()) return;
            if(clickedThing.def.IsWeapon && clickedThing.HasComp<Comp_AddHediffOnEquip>() && IsThingEquippable(clickedThing, context)) {
                __result = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Equip".Translate(clickedThing.LabelShort), delegate {
                    Find.WindowStack.Add(new Dialog_MessageBox("WindowEquipCursedBladeDesc".Translate(), "Yes".Translate(), delegate {
                        Equip(clickedThing, context);

                    }, "No".Translate()));
                }, MenuOptionPriority.High), context.FirstSelectedPawn, clickedThing);
            }
        }

        private static void Equip(Thing clickedThing, FloatMenuContext context) {
            clickedThing.SetForbidden(value: false);
            context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Equip, clickedThing), JobTag.Misc);
            FleckMaker.Static(clickedThing.DrawPos, clickedThing.MapHeld, FleckDefOf.FeedbackEquip);
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EquippingWeapons, KnowledgeAmount.Total);
        }

        //I have to rerun the same conditions as the original, otherwise the weapon might be equippable even if it's not supposed. I am also not willing to do another transpiler patch (not right now)
        private static bool IsThingEquippable(Thing clickedThing, FloatMenuContext context) {
            if(context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Violent)) return false;
            if(!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly)) return false;
            if(!context.FirstSelectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)) return false;
            if(clickedThing.IsBurning()) return false;
            if(context.FirstSelectedPawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(clickedThing, context.FirstSelectedPawn)) return false;
            if(!EquipmentUtility.CanEquip(clickedThing, context.FirstSelectedPawn, out var cantReason, checkBonded: false)) return false;
            return true;
        }
    }
}
