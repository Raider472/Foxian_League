using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class RitualRoleSacrificialOrganizer: RitualRole {
        public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false) {
            if(!AppliesIfChild(p, out reason, skipReason)) {
                return false;
            }
            if(!p.Faction.IsPlayerSafe()) {
                if(!skipReason) {
                    reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
                }
                return false;
            }
            if(ritual != null) {
                if(p == ritual.Organizer) {
                    return true;
                }
            }
            else if(assignments != null && assignments.Required(p)) {
                return true;
            }
            if(!MeditationFocusDefOf.Natural.CanPawnUse(p)) {
                if(!skipReason) {
                    reason = "RitualTargetAnimaTreeMustBeCapableOfNature".Translate();
                }
                return false;
            }
            if(!p.psychicEntropy.IsPsychicallySensitive) {
                if(!skipReason) {
                    reason = "RitualTargetAnimaTreeMustBePsychicallySensitive".Translate();
                }
                return false;
            }
            if(!CanPawnReceivesSacrificialBlessings(p)) {
                if(!skipReason) {
                    reason = "PawnCannotReceiveSacrificialBlessings".Translate();
                }
                return false;
            }
            return true;
        }

        public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false) {
            reason = null;
            return false;
        }

        private bool CanPawnReceivesSacrificialBlessings(Pawn p) {
            List<HediffDef> HediffsRitual = new List<HediffDef>();
            HediffsRitual.AddRange(RewardSacrificialRitual.GoodHediffs);
            HediffsRitual.AddRange(RewardSacrificialRitual.BadHediffs);
            foreach(HediffDef hediff in HediffsRitual) {
                if(p.health.hediffSet.HasHediff(hediff)) {
                    return false;
                }
            }
            return true;
        }
}
}
