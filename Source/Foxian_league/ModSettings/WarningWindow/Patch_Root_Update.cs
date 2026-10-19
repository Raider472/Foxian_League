using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Patch so that Root_Update can show the window if the conditions are fullfilled
    [HarmonyPatch(typeof(Root), "Update")]
    public static class Patch_Root_Update {
        public static void Postfix() {
            if(Patch_Game_FinalizeInit.showWarningWindow) {
                Find.WindowStack.Add(new Warning_Window());
                Patch_Game_FinalizeInit.showWarningWindow = false;
            }
        }
    }
}
