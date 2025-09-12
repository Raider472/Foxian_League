using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    //This whole mod settings was quite tricky to understand, so I checked how other mods did and thus got "inspired" by them
    //Inspiration: Nudist Evasion / Uncompromising Fire / Revia Race / Kijin Race
    internal class Foxian_Settings : ModSettings {
        public static float psychicManipulationFactor;

        public static EnumSkills skillEnum;

        public static float animalConnectionFactorTrigger;

        public Foxian_Settings() {
            SetDefaultValue(); 
        }

        private static void SetDefaultValue() {
            psychicManipulationFactor = 0.25f;
            skillEnum = EnumSkills.Social;
            animalConnectionFactorTrigger = 1.5f;
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref psychicManipulationFactor, "FLPsychicManipulationFactor", 0.25f);
            Scribe_Values.Look(ref skillEnum, "FLPsychicManipulationSkill", EnumSkills.Social);
            Scribe_Values.Look(ref animalConnectionFactorTrigger, "FLAnimalConnectionFactorTrigger", 1.5f);
        }

        internal static void WindowContents(Rect inRect) {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.Label("FoxianModSettingsWarning".Translate());
            listing_Standard.Gap(10f);
            if (listing_Standard.ButtonTextLabeled("PsychicManipulationSettingSkillsDropDown".Translate(), TranslateDropDownSkill(skillEnum), tooltip: "PsychicManipulationSettingSkillsDropDownDesc".Translate())) {
                List<FloatMenuOption> dropdownList = new List<FloatMenuOption>();
                foreach (EnumSkills chosenOption in Enum.GetValues(typeof(EnumSkills))) {
                    dropdownList.Add(new FloatMenuOption(TranslateDropDownSkill(chosenOption), delegate {
                        if (skillEnum != chosenOption) {
                            skillEnum = chosenOption;
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(dropdownList));
            }
            listing_Standard.Label(string.Concat("PsychicManipulationSettings".Translate() + ": ", (psychicManipulationFactor * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "PsychicManipulationSettingsDesc".Translate());
            psychicManipulationFactor = (float)Math.Round(listing_Standard.Slider(psychicManipulationFactor, 0.01f, 1f), 2);
            listing_Standard.Gap(30f);
            listing_Standard.Label(string.Concat("AnimalConnectionSettings".Translate() + ": ", (animalConnectionFactorTrigger * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "AnimalConnectionSettingsDesc".Translate());
            animalConnectionFactorTrigger = (float)Math.Round(listing_Standard.Slider(animalConnectionFactorTrigger, 0f, 2f), 2);
            listing_Standard.Gap(20f);
            if (listing_Standard.ButtonText("ResetButton".Translate())) {
                SetDefaultValue();
            }
            listing_Standard.End();
        }

        private static string TranslateDropDownSkill(EnumSkills skill) {
            return skill switch {
                EnumSkills.Construction => "PsychicManipConstruction".Translate(),
                EnumSkills.Plants => "PsychicManipPlants".Translate(),
                EnumSkills.Intellectual => "PsychicManipIntellectual".Translate(),
                EnumSkills.Mining => "PsychicManipMining".Translate(),
                EnumSkills.Shooting => "PsychicManipShooting".Translate(),
                EnumSkills.Melee => "PsychicManipMelee".Translate(),
                EnumSkills.Social => "PsychicManipSocial".Translate(),
                EnumSkills.Animals => "PsychicManipAnimals".Translate(),
                EnumSkills.Cooking => "PsychicManipCooking".Translate(),
                EnumSkills.Medicine => "PsychicManipMedicine".Translate(),
                EnumSkills.Artistic => "PsychicManipArtistic".Translate(),
                EnumSkills.Crafting => "PsychicManipCrafting".Translate(),
                _ => "Error"
            };
        }
    }
}
