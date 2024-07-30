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
            Log.Message("Foxian League Constructor Test V4");
            Harmony harmony = new Harmony("rimworld.foxian.league.raider");
            harmony.PatchAll();
        }
    }
}
