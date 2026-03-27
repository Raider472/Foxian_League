using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    public class Comp_vulyakkoResilience : ThingComp {
        //Comp to give a chance to Vulyakkos to reduce incoming damage by a percentage
        public CompProperties_vulyakkoResilience Props => (CompProperties_vulyakkoResilience)props;

        public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed) {
            base.PostPreApplyDamage(ref dinfo, out absorbed);
            if(Rand.Chance(Props.chanceToProc)) {
                float randomValue = (Rand.Value * (Props.maxPercentageReduction - Props.minPercentageReduction) + Props.minPercentageReduction);
                Log.Message($"Ability has proced with damage = {dinfo.Amount} and was it absorbed ? {absorbed}");
                int valueToReduce = Mathf.RoundToInt(dinfo.Amount * randomValue);
                dinfo.SetAmount(dinfo.Amount - valueToReduce);
                Log.Message($"New value set is: {dinfo.Amount} and reductin value was: {randomValue}");
            }
            else {
                Log.Message($"damage postPreApply = {dinfo} and is absorbed ? {absorbed}");
            }
        }

        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt) {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
            Log.Message($"damage postPostApply = {dinfo} and total damage = {totalDamageDealt}");
        }
    }
}
