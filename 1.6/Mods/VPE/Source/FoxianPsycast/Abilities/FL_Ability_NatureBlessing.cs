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
    public class FL_Ability_NatureBlessing : VEF.Abilities.Ability_Spawn {
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) {
            AbilityExtension_Spawn modExtension = def.GetModExtension<AbilityExtension_Spawn>();
            Pawn targetPawn = target.Pawn;
            if(modExtension == null || modExtension?.thing == null || targetPawn == null) return false;

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
            base.Cast(targets);
            foreach(GlobalTargetInfo target in targets) {
                Plant plant = target.Cell.GetPlant(pawn.Map);
                plant?.Kill();
                if(target.Pawn != null) {
                    DamageInfo damage = new DamageInfo(DamageDefOf.Cut, 100, instigator: pawn, intendedTarget: target.Pawn);
                    if (target.Pawn.Faction.PlayerRelationKind != FactionRelationKind.Hostile && !(target.Pawn.Faction.defeated)) {
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
