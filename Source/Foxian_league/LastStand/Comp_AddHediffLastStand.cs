using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Comp_AddHediffLastStand : CompAbilityEffect_GiveHediff {
        //Comp Ability to drain the psyfocus of target when it activates the ability
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest) {
            base.Apply(target, dest);
            if (parent.pawn.psychicEntropy.NeedsPsyfocus) {
                Log.Message("Amogus Last Stand");
                parent.pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
            }
        }

    }
}
