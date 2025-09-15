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

        public static bool isPawnFoxianTrigger;
        public static string xenotypeA;
        public static string xenotypeB;
        public static float xenotypeAChance;
        public static float xenotypeAMaleChance;

        public Foxian_Settings() {
            SetDefaultValue(); 
        }

        private static void SetDefaultValue() {
            psychicManipulationFactor = 0.25f;
            skillEnum = EnumSkills.Social;
            animalConnectionFactorTrigger = 1.5f;
            isPawnFoxianTrigger = true;
            xenotypeA = "FL_Foxian";
            xenotypeB = "FL_Greater_Foxian";
            xenotypeAChance = 0.7f;
            xenotypeAMaleChance = 0.7f;
        }

        public override void ExposeData() {
            base.ExposeData();
            Scribe_Values.Look(ref psychicManipulationFactor, "FLPsychicManipulationFactor", 0.25f);
            Scribe_Values.Look(ref skillEnum, "FLPsychicManipulationSkill", EnumSkills.Social);
            Scribe_Values.Look(ref animalConnectionFactorTrigger, "FLAnimalConnectionFactorTrigger", 1.5f);
            Scribe_Values.Look(ref isPawnFoxianTrigger, "FLGeneticPurityTrigger", true);
            Scribe_Values.Look(ref xenotypeA, "xenotypeA", "FL_Foxian");
            Scribe_Values.Look(ref xenotypeB, "xenotypeB", "FL_Greater_Foxian");
            Scribe_Values.Look(ref xenotypeAChance, "FLGeneticPurityXenotypeAChance", 0.7f);
            Scribe_Values.Look(ref xenotypeAMaleChance, "FLGeneticPurityXenotypeAMaleChance", 0.7f);
        }

        internal static void WindowContents(Rect inRect) {
            if (DefDatabase<XenotypeDef>.GetNamedSilentFail(xenotypeA) == null) xenotypeA = "FL_Foxian";
            if (DefDatabase<XenotypeDef>.GetNamedSilentFail(xenotypeB) == null) xenotypeB = "FL_Greater_Foxian";
            List<XenotypeDef> allXenotype = DefDatabase<XenotypeDef>.AllDefsListForReading;
            //if(!allXenotype.Contains(xenotypeA)) xenotypeA = InternalDefOf.FL_Foxian;
            //if (!allXenotype.Contains(xenotypeB)) xenotypeB = InternalDefOf.FL_Greater_Foxian;

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.Label("FoxianModSettingsWarning".Translate());
            listing_Standard.Label("FoxianInformation".Translate(), tooltip: "FoxianInformationDesc".Translate());
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
            listing_Standard.Gap(30f);
            listing_Standard.CheckboxLabeled("GeneticPurityFoxianTrigger".Translate(), ref isPawnFoxianTrigger, tooltip: "GeneticPurityFoxianTriggerDesc".Translate());
            if (listing_Standard.ButtonTextLabeled("GeneticPuritySettingXenotypeA".Translate(), DefDatabase<XenotypeDef>.GetNamed(xenotypeA).label, tooltip: "GeneticPuritySettingXenotypeADesc".Translate())) {
                List<FloatMenuOption> dropdownList = new List<FloatMenuOption>();
                foreach (XenotypeDef chosenXenoOption in allXenotype) {
                    dropdownList.Add(new FloatMenuOption(chosenXenoOption.label, delegate {
                        if (xenotypeA != chosenXenoOption.defName) {
                            xenotypeA = chosenXenoOption.defName ;
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(dropdownList));
            }
            listing_Standard.Gap(5f);
            if (listing_Standard.ButtonTextLabeled("GeneticPuritySettingXenotypeB".Translate(), DefDatabase<XenotypeDef>.GetNamed(xenotypeB).label, tooltip: "GeneticPuritySettingXenotypeBDesc".Translate())) {
                List<FloatMenuOption> dropdownList = new List<FloatMenuOption>();
                foreach (XenotypeDef chosenXenoOption in allXenotype) {
                    dropdownList.Add(new FloatMenuOption(chosenXenoOption.label, delegate {
                        if (xenotypeB != chosenXenoOption.defName) {
                            xenotypeB = chosenXenoOption.defName;
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(dropdownList));
            }
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
