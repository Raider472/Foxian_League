using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Abilities;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FoxianPsycast {
    public class FL_Ability_NatureBlessing : VEF.Abilities.Ability_Spawn {
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) {
            AbilityExtension_Spawn modExtension = def.GetModExtension<AbilityExtension_Spawn>();
            Pawn targetPawn = target.Pawn;
            if(modExtension == null || modExtension?.thing == null || targetPawn == null) return false;

            TerrainDef terrain = target.Cell.GetTerrain(pawn.Map);

            if(!terrain.IsSoil) {
                if(showMessages) {
                    Messages.Message("CannotPlantMissingTerrainTag".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(!targetPawn.RaceProps.Humanlike) {
                if(showMessages) {
                    Messages.Message("AbilityTargetMustBeHumanlike".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(targetPawn.Dead) {
                if(showMessages) {
                    Messages.Message("AbilityTargetMustBeAlive".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(!targetPawn.Downed) {
                if(showMessages) {
                    Messages.Message("AbilityTargetMustBeDowned".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return base.ValidateTarget(target, showMessages);
        }

        public override void Cast(params GlobalTargetInfo[] targets) {
            if(targets == null || targets.Length == 0) return;
            Log.Message($"Count: {targets.Length}");
            Plant plant = targets.First().Cell.GetPlant(pawn.Map);
            Log.Message($"Plant: {plant?.def?.defName}");
            plant?.Kill();

            base.Cast(targets);

            foreach(GlobalTargetInfo target in targets) {
                if(target.Pawn != null) {
                    DamageInfo damage = new DamageInfo(DamageDefOf.Cut, 100, instigator: pawn, intendedTarget: target.Pawn);
                    Faction faction = target.Pawn.Faction;
                    if (faction != null && faction != Faction.OfPlayer && Faction.OfPlayer.RelationKindWith(faction) != FactionRelationKind.Hostile && !(faction.defeated)) {
                        Faction.OfPlayer.TryAffectGoodwillWith(target.Pawn.Faction, -25, canSendMessage: true, canSendHostilityLetter: true, HistoryEventDefOf.UsedHarmfulAbility);
                    }
                    target.Pawn.Kill(damage);
                    target.Pawn.Corpse.Destroy();
                    Log.Message($"Target cell and map: {target.Cell}, {target.Map}");
                } 
            }
        }
    }
}
