using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Abilities;
using Verse;
using Verse.AI;
using Verse.Noise;
using static UnityEngine.GraphicsBuffer;

namespace FoxianPsycast {
    public class FL_Ability_PlantNaturaTree : VEF.Abilities.Ability_Spawn {
        //Ability to spawn a Natura tree, there are many checks in the ValidateTarget method to replicate the behaviour of planting a Natura seed

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) {
            AbilityExtension_Spawn modExtension = def.GetModExtension<AbilityExtension_Spawn>();
            if(modExtension == null || modExtension?.thing == null) return false;

            Thing item = target.Cell.GetFirstItem(pawn.Map);
            TerrainDef terrain = target.Cell.GetTerrain(pawn.Map);
            Thing blockingThing = PlantUtility.AdjacentSowBlocker(modExtension.thing, target.Cell, pawn.Map);
            Thing blockingPlant = target.Cell.GetPlant(pawn.Map);

            if(!target.Cell.IsValid || target.Cell.Fogged(pawn.Map)) {
                if(showMessages) {
                    Messages.Message("InvalidLocationPlant".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(target.Cell.Roofed(pawn.Map)) {
                if(showMessages) {
                    Messages.Message("AbilityRoofed".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(item != null) {
                if(showMessages) {
                    Messages.Message("BlockedBy".Translate(item), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(terrain.IsFloor) {
                if(showMessages) {
                    Messages.Message("CannotPlantMissingTerrainTag".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(terrain.fertility < modExtension.thing.plant?.fertilityMin) {
                if(showMessages) {
                    Messages.Message("MessageWarningNotEnoughFertility".Translate() + " " + "MessageMinFertilityRequired".Translate(modExtension.thing.plant.fertilityMin.ToStringPercent()), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(blockingThing != null) {
                if(showMessages) {
                    Messages.Message("AdjacentSowBlocker".Translate(blockingThing), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(blockingPlant != null && blockingPlant.def.plant.wildOrder >= 2) {
                if(showMessages) {
                    Messages.Message("CannotPlantInsideOtherPlant".Translate(blockingPlant), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(modExtension.thing.plant.minSpacingBetweenSamePlant > 0) {
                foreach(Thing itemPlant in pawn.Map.listerThings.ThingsOfDef(modExtension.thing)) {
                    if(itemPlant.Position.InHorDistOf(target.Cell, modExtension.thing.plant.minSpacingBetweenSamePlant)) {
                        if(showMessages) {
                            Messages.Message("TooCloseToOtherPlant".Translate(itemPlant), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                        }
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
