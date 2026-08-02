using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    public class HediffSeverityScalingStat : HediffWithComps {
        private float lastSavedStat = 0f;
        public override float Severity {
            get {
                return base.severityInt;
            }
            set {
                base.Severity = value;
            } 
        }

        public override void TickInterval(int delta) {
            base.TickInterval(delta);
            if(pawn.IsHashIntervalTick(240, delta) && pawn.GetStatValue(def.GetModExtension<HediffExtension_ScalableSeverityStat>().scalingStat) != lastSavedStat) {
                UpdateSeverity();
            }
        }

        public override bool ShouldRemove => base.ShouldRemove && (def.GetModExtension<HediffExtension_ScalableSeverityStat>()?.shouldBeRemovedWhenZero ?? false);

        private void UpdateSeverity() {
            HediffExtension_ScalableSeverityStat hediffExtension = def.GetModExtension<HediffExtension_ScalableSeverityStat>();
            if(hediffExtension == null) {
                base.severityInt = 0;
                return;
            }
            Log.Message($"name: {pawn}, stats: {hediffExtension.scalingStat}, value of limits: {hediffExtension.statLimit.min} to {hediffExtension.statLimit.max}");
            float statValue = pawn.GetStatValue(hediffExtension.scalingStat);
            lastSavedStat = statValue;
            float normalizedValue = Mathf.Clamp((statValue - hediffExtension.statLimit.min) / (hediffExtension.statLimit.max - hediffExtension.statLimit.min), def.minSeverity, def.maxSeverity);
            Log.Message($"normalized value: {normalizedValue}");
            Severity = normalizedValue;
        }

        public override string LabelInBrackets {
            get {
                string labelInBrackets = base.LabelInBrackets;
                string text = Severity.ToStringPercent("F0");
                if(labelInBrackets.Length == 0) return text;
                else {
                    return labelInBrackets + " - " + text;
                }
            }
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref lastSavedStat, "lastSavedStat", 0f);
        }
    }
}
