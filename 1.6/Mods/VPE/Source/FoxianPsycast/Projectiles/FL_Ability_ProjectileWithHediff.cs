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
    public class FL_Ability_ProjectileWithHediff : VEF.Abilities.AbilityProjectile {
        protected override void Impact(Thing hitThing, bool blockedByShield = false) {
            base.Impact(hitThing);
            AbilityExtension_ProjectileWithHediff modExtension = def.GetModExtension<AbilityExtension_ProjectileWithHediff>();
            if(hitThing == null || !(hitThing is Pawn) || modExtension == null) return;
            Pawn target = hitThing as Pawn;
            if(target.IsShambler || target.RaceProps.IsMechanoid || target.Dead) return;
            Hediff hediffToChange;
            Log.Message($"Here is max and min {modExtension.severityRange.max} - {modExtension.severityRange.min}");
            float randomValue = (Rand.Value * (modExtension.severityRange.max - modExtension.severityRange.min) + modExtension.severityRange.min);
            if(target.kindDef == PawnKindDefOf.Thrumbo || target.kindDef == PawnKindDefOf.AlphaThrumbo) randomValue /= 10f;
            if(target.health.hediffSet.TryGetHediff(modExtension.hediffOnHit, out hediffToChange)) {
                hediffToChange.Severity += randomValue;
                if(hediffToChange.Severity > 1f) {
                    hediffToChange.Severity = 1f;
                }
            }
            else {
                hediffToChange = HediffMaker.MakeHediff(modExtension.hediffOnHit, target, null);
                hediffToChange.Severity = randomValue;
                target.health.AddHediff(hediffToChange);
            }
        }
    }
}
