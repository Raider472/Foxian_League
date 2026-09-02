using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Abilities;
using Verse;

namespace FoxianPsycast {
    public class FL_ability_PlantWoodMine : VEF.Abilities.Ability_Spawn {
        //Ability to spawn a wood mine trap, there are many checks in the ValidateTarget method to replicate the behaviour of planting an IED trap
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) {
            AbilityExtension_Spawn modExtension = def.GetModExtension<AbilityExtension_Spawn>();
            if(modExtension == null || modExtension?.thing == null) return false;

            Thing item = target.Cell.GetFirstItem(pawn.Map);
            TerrainDef terrain = target.Cell.GetTerrain(pawn.Map);
            Thing blockingPlant = target.Cell.GetPlant(pawn.Map);

            if(!target.Cell.IsValid || target.Cell.Fogged(pawn.Map)) {
                if(showMessages) {
                    Messages.Message("InvalidLocationPlant".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(item != null) {
                if(showMessages) {
                    Messages.Message("BlockedBy".Translate(item), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(!terrain.IsSoil) {
                if(showMessages) {
                    Messages.Message("CannotPlantMissingTerrainTag".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(blockingPlant != null && blockingPlant.def.plant.wildOrder >= 2) {
                if(showMessages) {
                    Messages.Message("CannotPlantInsideOtherPlant".Translate(blockingPlant), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            foreach(Thing itemPlant in pawn.Map.listerThings.ThingsOfDef(modExtension.thing)) {
                if(itemPlant.Position.InHorDistOf(target.Cell, 1.9f)) {
                    if(showMessages) {
                        Messages.Message("TooCloseToOtherPlant".Translate(itemPlant), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                    }
                    return false;
                }
            }
            for(int i = 0; i < 8; i++) {
                IntVec3 c2 = target.Cell + GenAdj.AdjacentCells[i];
                if(c2.InBounds(pawn.Map)) {
                    Thing trap = c2.GetFirstThing<Thing>(pawn.Map);
                    Log.Message("Checking adjacent cell: " + c2 + " for traps. Found: " + (trap != null ? trap.Label : "none"));
                    if(trap != null && trap.def.building != null && trap.def.building.isTrap) {
                        if(showMessages) Messages.Message("CannotPlaceAdjacentTrap".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                        return false;
                    }
                }
            }
            return base.ValidateTarget(target, showMessages);
        }

        public override void Cast(params GlobalTargetInfo[] targets) {
            if(targets == null || targets.Length == 0) {
                base.Cast(targets);
                return;
            }
            for(int i = 0; i < targets.Length; i++) {
                Plant plant = targets[i].Cell.GetPlant(pawn.Map);
                if(plant != null) {
                    plant.Kill();
                }
            }
            base.Cast(targets);
        }
    }
}
