using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public class RitualObligationTargetWorker_SacrificialTree : RitualObligationTargetFilter {
        public RitualObligationTargetWorker_SacrificialTree() {
        }

        public RitualObligationTargetWorker_SacrificialTree(RitualObligationTargetFilterDef def)
            : base(def) 
        {
        }

        public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map) {
            return Enumerable.Empty<TargetInfo>();
        }

        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation) {
            if(target.Thing.def != InternalDefOf.FL_Plant_SacrificialTree) {
                return false;
            }
            bool flag = false;
            bool flag2 = false;
            foreach(Pawn item in target.Map.mapPawns.FreeColonistsSpawned) {
                if(MeditationFocusDefOf.Natural.CanPawnUse(item)) {
                    flag2 = true;
                }
            }

            Plant plant = target.Cell.GetPlant(target.Map);
            if(plant.LifeStage == PlantLifeStage.Mature) flag = true;

            if(!flag) {
                return "RitualTargetSacrificialTreeNotMature".Translate();
            }
            if(!flag2) {
                return "RitualTargetAnimaTreeNoPawnsWithNatureFocus".Translate();
            }

            return true;
        }

        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation) {
            yield return "RitualTargetSacrificialTreeInfo".Translate(); ;
        }
    }
}
