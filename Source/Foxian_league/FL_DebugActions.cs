using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public static class FL_DebugActions {
        //Copied the code from ReSplice Charmweaver, because I grew tired of waiting for an update for this mod
        [DebugAction("Pawns", "Set pregnancy at 99%", false, false, true, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetPregnancyAt99(Pawn p) {
            Hediff hediff = p.health.hediffSet.hediffs.FirstOrDefault((Hediff x) => x.def == HediffDefOf.PregnantHuman && x.Severity < 0.99f);
            if (hediff != null) {
                hediff.Severity = 0.99f;
            }
        }
    }
}
