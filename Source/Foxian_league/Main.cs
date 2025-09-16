using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace Foxian_league {
    [StaticConstructorOnStartup]
    public class Main {
        static Main() {
            Harmony harmony = new Harmony("rimworld.foxian.league.raider");
            harmony.PatchAll();
            if (DefDatabase<XenotypeDef>.GetNamedSilentFail(Foxian_Settings.xenotypeA) == null) Foxian_Settings.xenotypeA = InternalDefOf.FL_Foxian.defName;
            if (DefDatabase<XenotypeDef>.GetNamedSilentFail(Foxian_Settings.xenotypeB) == null) Foxian_Settings.xenotypeB = InternalDefOf.FL_Greater_Foxian.defName;
        }
    }
}
