using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class CompAbilityEffect_FateManipulation: CompAbilityEffect {
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
            IncidentDef.Named("Infestation"),
            IncidentDef.Named("HeatWave"),
            IncidentDef.Named("ColdSnap"),
            IncidentDef.Named("PsychicDrone"),
            IncidentDef.Named("SolarFlare"),
            IncidentDef.Named("Disease_Plague"),
            IncidentDef.Named("AnimalInsanityMass"),
            IncidentDef.Named("AnimalInsanitySingle"),
        };

        public override void PostApplied(List<LocalTargetInfo> targets, Map map) {
            Log.Message($"Fate Manipulation effect applied to targets: {string.Join(", ", targets.Select(t => t.Thing?.Label ?? "unknown target"))}");
            base.PostApplied(targets, map);
            float backupChance = chanceForExcelentEvent;
            IncidentDef incident = GetIncidentListBasedOnChance().RandomElement();
            Log.Message($"Selected incident: {incident.defName}");
            Log.Message($"Current chance for excellent event: {chanceForExcelentEvent}");
            IncreaseChanceBasedOnIncident(incident);
            Log.Message($"Chance for excellent event after increase: {chanceForExcelentEvent}");
            TryIncidentRecursive(incident, map, backupChance);
        }

        private List<IncidentDef> GetIncidentListBasedOnChance() {
            List<IncidentDef> incidentList = new List<IncidentDef>();
            if(chanceForExcelentEvent >= 1f) return ExcellentIncidents;
            incidentList.AddRange(ExcellentIncidents);
            incidentList.AddRange(GoodIncidents);
            incidentList.AddRange(NeutralOrBadIncidents);
            Log.Message($"Chance for excellent event is {chanceForExcelentEvent}, so ExcellentIncidents, GoodIncidents, and NeutralOrBadIncidents are included in the incident list.");
            if(chanceForExcelentEvent <= 0.8f) {
                incidentList.AddRange(VeryBadIncidents);
                Log.Message($"Chance for excellent event is {chanceForExcelentEvent}, so VeryBadIncidents are included in the incident list.");
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
            chanceForExcelentEvent = Math.Clamp(chanceForExcelentEvent, 0f, 1f);
        }

        private void TryIncidentRecursive(IncidentDef incident, Map map, float backupChance, int recursiveLoop = 10) {
            if(recursiveLoop <= 0) {
                Log.Error("Max recursive attempts reached. No incident could be executed.");
                return;
            }
            if(!incident.Worker.TryExecute(StorytellerUtility.DefaultParmsNow(incident.category, map))) {
                Log.Message($"Chance for excellent event: {chanceForExcelentEvent} and chance for fallback: {backupChance} before fallback, and recursive attempts left: {recursiveLoop}");
                chanceForExcelentEvent = backupChance; // Revert chance if fallback incident is executed
                IncidentDef fallbackIncident = GetIncidentListBasedOnChance().RandomElement();
                Log.Message($"Selected fallback incident: {fallbackIncident.defName}");
                IncreaseChanceBasedOnIncident(fallbackIncident);
                Log.Message($"Chance for excellent event after new increase: {chanceForExcelentEvent}");
                TryIncidentRecursive(fallbackIncident, map, backupChance, recursiveLoop - 1);
            }
        }

        public override void PostExposeData() {
            base.PostExposeData();
            Scribe_Values.Look(ref chanceForExcelentEvent, "chanceForExcelentEvent", 0f);
        }
    }
}
