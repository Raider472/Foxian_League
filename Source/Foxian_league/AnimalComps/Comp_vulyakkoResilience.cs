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
                int valueToReduce = Mathf.RoundToInt(dinfo.Amount * randomValue);
                dinfo.SetAmount(dinfo.Amount - valueToReduce);
            }
            else {
            }
        }

        //I think I used this override to log the damage reduction but I am not sure anymore ?
        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt) {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
        }
    }
}
