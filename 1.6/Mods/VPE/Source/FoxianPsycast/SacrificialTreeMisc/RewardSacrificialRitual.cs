using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FoxianPsycast {
    public static class RewardSacrificialRitual {
        public static List<HediffDef> GoodHediffs = new List<HediffDef> {
            InternalDefOf.FL_BlessingHarmony,
            InternalDefOf.FL_BlessingFortitude,
            InternalDefOf.FL_DummyHediffWarior
        };

        public static List<HediffDef> BadHediffs = new List<HediffDef> {
            InternalDefOf.FL_CurseRot,
            InternalDefOf.FL_NightmareTorment,
            InternalDefOf.FL_CurseSubmission,
            InternalDefOf.FL_CurseDrain
        };

        public static List<IncidentDef> Incidents = new List<IncidentDef> {
            IncidentDefOf.Drought,
            IncidentDefOf.ToxicFallout,
            IncidentDefOf.SolarFlare,
            IncidentDef.Named("Disease_Plague"),
            IncidentDef.Named("ColdSnap"),
            IncidentDef.Named("Eclipse"),
        };

        //TODO Add new items just in case
        public static List<ThingDef> RareItems = new List<ThingDef> {
            ThingDef.Named("FL_Weapoon_Naginata_BladeLink"),
            ThingDef.Named("FL_Weapon_CleansedSword"),
            ThingDef.Named("FL_Weapon_CursedSword"),
            ThingDef.Named("FL_Weapoon_Oracle_Staff"),
            ThingDef.Named("MechSerumHealer"), 
            ThingDef.Named("PsychicSensitizer"),
            ThingDef.Named("OrbitalTargeterPowerBeam"),
        };
    }
}
