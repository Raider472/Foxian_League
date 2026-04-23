using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class IncidentWorker_NaturaPodSpawn : IncidentWorker {
        protected override bool CanFireNowSub(IncidentParms parms) {
            if(base.CanFireNowSub(parms)) {
                Map map = (Map)parms.target;
                return anyColonistsHasNaturalPsySensitivity(map.mapPawns.FreeColonists);
            }
            return false;
        }

        protected override bool TryExecuteWorker(IncidentParms parms) {
            Map map = (Map)parms.target;
            if(!TryFindRootCell(map, out var cell)) {
                return false;
            }
            if(!TrySpawnAt(cell, map, out var plant)) {
                return false;
            }
            ((Plant)plant).Growth = 1f;
            SendStandardLetter(parms, plant);
            return true;
        }

        private bool anyColonistsHasNaturalPsySensitivity(List<Pawn> colonists) {
            foreach(Pawn pawn in colonists) {
                if(Utils.HasActiveGene(pawn, InternalDefOf.FL_NaturalPsySensitive)) return true;
            }
            return false;
        }

        private static bool CanSpawnPodAt(IntVec3 c, Map map) {
            if(!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map).PsychologicallyOutdoors || c.Roofed(map)) {
                return false;
            }
            Plant plant = c.GetPlant(map);
            if(plant != null && plant.def.plant.growDays > 10f) {
                return false;
            }
            List<Thing> thingList = c.GetThingList(map);
            for(int i = 0; i < thingList.Count; i++) {
                if(thingList[i].def == InternalDefOf.FL_Plant_PodNatura) {
                    return false;
                }
            }
            if(!map.reachability.CanReachFactionBase(c, map.ParentFaction)) {
                return false;
            }
            if(c.GetTerrain(map).avoidWander) {
                return false;
            }
            if(c.GetFertility(map) < InternalDefOf.FL_Plant_PodNatura.plant.fertilityMin) {
                return false;
            }
            return true;
        }

        public static bool TryFindRootCell(Map map, out IntVec3 cell) {
            return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => CanSpawnPodAt(x, map), map, out cell);
        }

        private bool TrySpawnAt(IntVec3 cell, Map map, out Thing plant) {
            cell.GetPlant(map)?.Destroy();
            plant = GenSpawn.Spawn(InternalDefOf.FL_Plant_PodNatura, cell, map);
            return plant != null;
        }
    }
}
