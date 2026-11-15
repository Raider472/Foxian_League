using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace FoxianPsycast {
    public class FL_Ability_ThornExplosion : VEF.Abilities.Ability_Explode {
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) {
            TerrainDef terrain = target.Cell.GetTerrain(pawn.Map);

            if(!target.Cell.IsValid || target.Cell.Fogged(pawn.Map)) {
                if(showMessages) {
                    Messages.Message("InvalidLocationPlant".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if(!terrain.IsSoil) {
                if(showMessages) {
                    Messages.Message("InvalidLocationPlant".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return base.ValidateTarget(target, showMessages);
        }
    }
}
