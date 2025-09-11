using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    //This whole mod settings was quite tricky to understand, so I checked how other mods did and thus got "inspired" by them
    //Inspiration: Nudist Evasion / Revia Race / Kijin Race
    internal class Foxian_Settings : ModSettings {
        public static float psychicManipulationFactor;
        private static string psychicManipulationFactorBuffer = string.Concat(psychicManipulationFactor);

        public Foxian_Settings() {
            SetDefaultValue(); 
        }

        private static void SetDefaultValue() {
            psychicManipulationFactor = 0.25f;
            psychicManipulationFactorBuffer = string.Concat(psychicManipulationFactor);
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref psychicManipulationFactor, "FLPsychicManipulationFactor", 0.25f);
        }

        internal static void WindowContents(Rect inRect) {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            //listing_Standard.TextFieldNumericLabeled("PsychicManipulationSettings".Translate(), ref psychicManipulationFactor, ref psychicManipulationFactorBuffer, 0.01f);
            listing_Standard.Label("FoxianModSettingsWarning".Translate());
            listing_Standard.Gap(5f);
            listing_Standard.Label(string.Concat("PsychicManipulationSettings".Translate() + ": ", (psychicManipulationFactor * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "PsychicManipulationSettingsDesc".Translate());
            psychicManipulationFactor = (float)Math.Round(listing_Standard.Slider(psychicManipulationFactor, 0.01f, 1f), 2);
            listing_Standard.Gap(2f);
            if (listing_Standard.ButtonText("Default Values")) {
                SetDefaultValue();
            }
            listing_Standard.End();
        }
    }
}
