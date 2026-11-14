using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {

    public class CompAbilityEffect_FateManipulation: CompAbilityEffect {
        //Comp that hold every incident and the logic for the fate manipulation ability.
        //It has an in built "pity" system to guarantee at least a good incident after a few trys
        public new CompProperties_AbilityEffectFateManipulation Props => (CompProperties_AbilityEffectFateManipulation)props;
        private float chanceForExcelentEvent = 0f;

        public List<IncidentDef> ExcellentIncidents => new List<IncidentDef>() {
            IncidentDef.Named("ThrumboPasses"),
            IncidentDef.Named("SelfTame"),
            IncidentDef.Named("WandererJoin"),
        };

        public List<IncidentDef> GoodIncidents => new List<IncidentDef> {
            IncidentDef.Named("ResourcePodCrash"),
            IncidentDef.Named("PsychicSoothe"),
            IncidentDef.Named("FarmAnimalsWanderIn"),
            IncidentDef.Named("RefugeePodCrash"),
            IncidentDef.Named("ShipChunkDrop"),
            IncidentDef.Named("TraderCaravanArrival"),
            IncidentDef.Named("OrbitalTraderArrival"),
            IncidentDef.Named("AmbrosiaSprout"),
        };

        public List<IncidentDef> NeutralOrBadIncidents => new List<IncidentDef> {
            IncidentDef.Named("MeteoriteImpact"),
            IncidentDef.Named("Flashstorm"),
            IncidentDef.Named("ShortCircuit"),
            IncidentDef.Named("CropBlight"),
            IncidentDef.Named("Disease_Flu"),
            IncidentDef.Named("VisitorGroup"),
            IncidentDef.Named("TravelerGroup"),
            IncidentDef.Named("WildManWandersIn"),
        };

        public List<IncidentDef> VeryBadIncidents => new List<IncidentDef> {
            IncidentDef.Named("HeatWave"),
            IncidentDef.Named("ColdSnap"),
            IncidentDef.Named("PsychicDrone"),
            IncidentDef.Named("SolarFlare"),
            IncidentDef.Named("Disease_Plague"),
            IncidentDef.Named("AnimalInsanityMass"),
            IncidentDef.Named("AnimalInsanitySingle"),
        };

        public override void PostApplied(List<LocalTargetInfo> targets, Map map) {
            base.PostApplied(targets, map);
            float backupChance = chanceForExcelentEvent;
            IncidentDef incident = GetIncidentListBasedOnChance().RandomElement();
            IncreaseChanceBasedOnIncident(incident);
            TryIncidentRecursive(incident, map, backupChance);
        }

        private List<IncidentDef> GetIncidentListBasedOnChance() {
            List<IncidentDef> incidentList = new List<IncidentDef>();
            if(chanceForExcelentEvent >= 1f) return ExcellentIncidents;
            incidentList.AddRange(ExcellentIncidents);
            incidentList.AddRange(GoodIncidents);
            incidentList.AddRange(NeutralOrBadIncidents);
            if(chanceForExcelentEvent <= 0.8f) {
                incidentList.AddRange(VeryBadIncidents);
            }
            return incidentList;
        }

        private void IncreaseChanceBasedOnIncident(IncidentDef incident) {
            if(ExcellentIncidents.Contains(incident)) {
                chanceForExcelentEvent = 0f;
            }
            else if(GoodIncidents.Contains(incident)) {
                chanceForExcelentEvent += Rand.Range(0.05f, 0.15f);
            }
            else if(NeutralOrBadIncidents.Contains(incident)) {
                chanceForExcelentEvent += Rand.Range(0.15f, 0.4f);
            }
            else if(VeryBadIncidents.Contains(incident)) {
                chanceForExcelentEvent += Rand.Range(0.4f, 0.7f);
            }
            chanceForExcelentEvent = Mathf.Clamp(chanceForExcelentEvent, 0f, 1f);
        }

        private void TryIncidentRecursive(IncidentDef incident, Map map, float backupChance, int recursiveLoop = 10) {
            if(recursiveLoop <= 0) {
                Log.Error("Max recursive attempts reached. No incident could be executed.");
                return;
            }
            if(!incident.Worker.TryExecute(StorytellerUtility.DefaultParmsNow(incident.category, map))) {
                chanceForExcelentEvent = backupChance; // Revert chance if fallback incident is executed
                IncidentDef fallbackIncident = GetIncidentListBasedOnChance().RandomElement();
                IncreaseChanceBasedOnIncident(fallbackIncident);
                TryIncidentRecursive(fallbackIncident, map, backupChance, recursiveLoop - 1);
            }
        }

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref chanceForExcelentEvent, "chanceForExcelentEvent", 0f);
        }
    }
}
