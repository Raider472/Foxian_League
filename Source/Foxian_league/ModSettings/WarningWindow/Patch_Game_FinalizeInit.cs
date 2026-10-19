using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Patch to FinalizeInit to set a flag once the player is done loading a game and if they never received the warning message
    [HarmonyPatch(typeof(Game), "FinalizeInit")]
    public static class Patch_Game_FinalizeInit {
        public static bool showWarningWindow = false;

        public static void Postfix() {
            if(!Foxian_Settings.hasWarningBeenShown) {
                showWarningWindow = true;
            }
        }
    }
}
