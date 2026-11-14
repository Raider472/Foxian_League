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
    //Title is misleading, this class does not apply an hediff but ensure that relations take a hit when a neutral pawn is hit by the spore infection ability
    public class FL_ApplyHediffsWithoutTimer : VEF.Abilities.Ability {
        public override void Cast(params GlobalTargetInfo[] targets) {
            //AbilityExtension_Hediff modExtension = def.GetModExtension<AbilityExtension_Hediff>();
            foreach(GlobalTargetInfo target in targets) {
                if(target.Thing is Pawn) {
                    Pawn pawn = (Pawn)target.Thing;

                    /*Hediff hediff = HediffMaker.MakeHediff(modExtension.hediff, pawn, pawn.health.hediffSet.GetBrain());
                    Log.Message($"Hediff: {hediff}");
                    pawn.health.AddHediff(hediff);*/

                    Faction faction = pawn.Faction;
                    if(faction != null && faction != Faction.OfPlayer && Faction.OfPlayer.RelationKindWith(faction) != FactionRelationKind.Hostile && !(faction.defeated)) {
                        Faction.OfPlayer.TryAffectGoodwillWith(faction, -10, canSendMessage: true, canSendHostilityLetter: true, HistoryEventDefOf.UsedHarmfulAbility);
                    }
                }
            }
            base.Cast(targets);
        }
    }
}
