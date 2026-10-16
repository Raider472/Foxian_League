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

        private static Vector2 scrollPosition = Vector2.zero;
        public static float psychicManipulationFactor;

        public static EnumSkills skillEnum;

        public static bool isPawnFoxianTrigger;
        public static string xenotypeA;
        public static string xenotypeB;
        public static float xenotypeAChance;
        public static float xenotypeMaleChance;
        public static float xenotypeFemaleChanceAlt;
        public static float maxProgressMultiplier;
        public static bool IsFoxianBi;

        public static float oneTailAgeValue;
        private static string oneTailAgeValueString;

        public static float threeTailAgeValue;
        private static string threeTailAgeValueString;

        public static float fiveTailAgeValue;
        private static string fiveTailAgeValueString;

        public static float sevenTailAgeValue;
        private static string sevenTailAgeValueString;

        public static float nineTailAgeValue;
        private static string nineTailAgeValueString;

        private static readonly List<float> TailAgeValuesReference = new List<float> { 0f, 15f, 18f, 21f, 28f };

        public Foxian_Settings() {
            SetDefaultValue();
        }

        private static void SetDefaultValue() {
            psychicManipulationFactor = 0.25f;
            skillEnum = EnumSkills.Social;
            isPawnFoxianTrigger = true;
            xenotypeA = "FL_Foxian";
            xenotypeB = "FL_Greater_Foxian";
            xenotypeAChance = 0.7f;
            xenotypeMaleChance = 0.7f;
            xenotypeFemaleChanceAlt = 0.8f;
            maxProgressMultiplier = 1f;
            IsFoxianBi = true;
            oneTailAgeValue = 0f;
            oneTailAgeValueString = oneTailAgeValue.ToString();
            threeTailAgeValue = 15f;
            threeTailAgeValueString = threeTailAgeValue.ToString();
            fiveTailAgeValue = 18f;
            fiveTailAgeValueString = fiveTailAgeValue.ToString();
            sevenTailAgeValue = 21f;
            sevenTailAgeValueString = sevenTailAgeValue.ToString();
            nineTailAgeValue = 28f;
            nineTailAgeValueString = nineTailAgeValue.ToString();
        }

        public override void ExposeData() {
            base.ExposeData();
            //Check if tails values are valide before saving them, correct them if they are not
            ValidateTailsAgeValues();

            Scribe_Values.Look(ref psychicManipulationFactor, "FLPsychicManipulationFactor", 0.25f);
            Scribe_Values.Look(ref skillEnum, "FLPsychicManipulationSkill", EnumSkills.Social);
            Scribe_Values.Look(ref isPawnFoxianTrigger, "FLGeneticPurityTrigger", true);
            Scribe_Values.Look(ref xenotypeA, "xenotypeA", "FL_Foxian");
            Scribe_Values.Look(ref xenotypeB, "xenotypeB", "FL_Greater_Foxian");
            Scribe_Values.Look(ref xenotypeAChance, "FLGeneticPurityXenotypeAChance", 0.7f);
            Scribe_Values.Look(ref xenotypeMaleChance, "FLGeneticPurityXenotypeAMaleChance", 0.7f);
            Scribe_Values.Look(ref xenotypeFemaleChanceAlt, "FLGeneticPurityXenotypeFemaleChanceAlt", 0.8f);
            Scribe_Values.Look(ref maxProgressMultiplier, "FLmaxProgressMultiplier", 1f);
            Scribe_Values.Look(ref IsFoxianBi, "FLIsFoxianBi", true);
            Scribe_Values.Look(ref oneTailAgeValue, "FLOneTailAgeValue", 0f);
            Scribe_Values.Look(ref threeTailAgeValue, "FLThreeTailAgeValue", 15f);
            Scribe_Values.Look(ref fiveTailAgeValue, "FLFiveTailAgeValue", 18f);
            Scribe_Values.Look(ref sevenTailAgeValue, "FLSevenTailAgeValue", 21f);
            Scribe_Values.Look(ref nineTailAgeValue, "FLNineTailAgeValue", 28f);

            if(Scribe.mode == LoadSaveMode.LoadingVars) {
                UpdateTailAgeValueStrings();
            }
        }

        internal static void WindowContents(Rect inRect) {
            if(DefDatabase<XenotypeDef>.GetNamedSilentFail(xenotypeA) == null) xenotypeA = "FL_Foxian";
            if(DefDatabase<XenotypeDef>.GetNamedSilentFail(xenotypeB) == null) xenotypeB = "FL_Greater_Foxian";
            List<XenotypeDef> allXenotype = DefDatabase<XenotypeDef>.AllDefsListForReading;
            //if(!allXenotype.Contains(xenotypeA)) xenotypeA = InternalDefOf.FL_Foxian;
            //if (!allXenotype.Contains(xenotypeB)) xenotypeB = InternalDefOf.FL_Greater_Foxian;

            //rect to add a scrollbar lol
            Rect viewRect = new Rect(
                0f,
                0f,
                inRect.width - 20f,
                1300f
            );

            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(viewRect);

            listing_Standard.Label("FoxianModSettingsWarning".Translate());
            listing_Standard.Label("FoxianInformation".Translate(), tooltip: "FoxianInformationDesc".Translate());
            listing_Standard.Gap(30f);

            listing_Standard.Label("FoxianLustfulGeneTitle".Translate());
            listing_Standard.Gap(-4f);
            listing_Standard.GapLine();
            listing_Standard.Gap(5f);

            listing_Standard.CheckboxLabeled("IsFoxianBi".Translate(), ref IsFoxianBi, tooltip: "IsFoxianBiDesc".Translate());
            listing_Standard.Gap(25f);

            listing_Standard.Label("FoxianPsychicManipGeneTitle".Translate());
            listing_Standard.Gap(-4f);
            listing_Standard.GapLine();
            listing_Standard.Gap(5f);

            if(listing_Standard.ButtonTextLabeled("PsychicManipulationSettingSkillsDropDown".Translate(), TranslateDropDownSkill(skillEnum), tooltip: "PsychicManipulationSettingSkillsDropDownDesc".Translate())) {
                List<FloatMenuOption> dropdownList = new List<FloatMenuOption>();
                foreach(EnumSkills chosenOption in Enum.GetValues(typeof(EnumSkills))) {
                    dropdownList.Add(new FloatMenuOption(TranslateDropDownSkill(chosenOption), delegate {
                        if(skillEnum != chosenOption) {
                            skillEnum = chosenOption;
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(dropdownList));
            }
            listing_Standard.Gap(15f);

            listing_Standard.Label(string.Concat("PsychicManipulationSettings".Translate() + ": ", (psychicManipulationFactor * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "PsychicManipulationSettingsDesc".Translate());
            psychicManipulationFactor = (float)Math.Round(listing_Standard.Slider(psychicManipulationFactor, 0.01f, 1f), 2);
            listing_Standard.Gap(25f);

            listing_Standard.Label("FoxianGeneticPurityTitle".Translate());
            listing_Standard.Gap(-4f);
            listing_Standard.GapLine();
            listing_Standard.Gap(5f);

            listing_Standard.CheckboxLabeled("GeneticPurityFoxianTrigger".Translate(), ref isPawnFoxianTrigger, tooltip: "GeneticPurityFoxianTriggerDesc".Translate());
            listing_Standard.Gap(15f);
            if(listing_Standard.ButtonTextLabeled("GeneticPuritySettingXenotypeA".Translate(), DefDatabase<XenotypeDef>.GetNamed(xenotypeA).label, tooltip: "GeneticPuritySettingXenotypeADesc".Translate())) {
                List<FloatMenuOption> dropdownList = new List<FloatMenuOption>();
                foreach(XenotypeDef chosenXenoOption in allXenotype) {
                    dropdownList.Add(new FloatMenuOption(chosenXenoOption.label, delegate {
                        if(xenotypeA != chosenXenoOption.defName) {
                            xenotypeA = chosenXenoOption.defName;
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(dropdownList));
            }
            listing_Standard.Gap(10f);

            if(listing_Standard.ButtonTextLabeled("GeneticPuritySettingXenotypeB".Translate(), DefDatabase<XenotypeDef>.GetNamed(xenotypeB).label, tooltip: "GeneticPuritySettingXenotypeBDesc".Translate())) {
                List<FloatMenuOption> dropdownList = new List<FloatMenuOption>();
                foreach(XenotypeDef chosenXenoOption in allXenotype) {
                    dropdownList.Add(new FloatMenuOption(chosenXenoOption.label, delegate {
                        if(xenotypeB != chosenXenoOption.defName) {
                            xenotypeB = chosenXenoOption.defName;
                        }
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(dropdownList));
            }
            listing_Standard.Gap(15f);

            listing_Standard.Label(string.Concat("GeneticPurityXenotypeAChanceSlider".Translate() + ": ", (xenotypeAChance * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "GeneticPurityXenotypeAChanceSliderDesc".Translate());
            xenotypeAChance = (float)Math.Round(listing_Standard.Slider(xenotypeAChance, 0f, 1f), 2);
            listing_Standard.Gap(15f);

            listing_Standard.Label(string.Concat("GeneticPurityXenotypeMaleChanceSlider".Translate() + ": ", (xenotypeMaleChance * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "GeneticPurityXenotypeMaleChanceSliderDesc".Translate());
            xenotypeMaleChance = (float)Math.Round(listing_Standard.Slider(xenotypeMaleChance, 0f, 1f), 2);
            listing_Standard.Gap(15f);

            listing_Standard.Label(string.Concat("GeneticPurityXenotypeFemaleChanceSliderAlt".Translate() + ": ", (xenotypeFemaleChanceAlt * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "GeneticPurityXenotypeFemaleChanceSliderAltDesc".Translate());
            xenotypeFemaleChanceAlt = (float)Math.Round(listing_Standard.Slider(xenotypeFemaleChanceAlt, 0f, 1f), 2);
            listing_Standard.Gap(25f);

            listing_Standard.Label("FoxianNaturaTreeTitle".Translate());
            listing_Standard.Gap(-4f);
            listing_Standard.GapLine();
            listing_Standard.Gap(5f);

            listing_Standard.Label(string.Concat("maxProgressTreeMultiplierSlider".Translate() + ": ", (maxProgressMultiplier * 100f).ToString(), "% ", "HoverForInfo".Translate()), tooltip: "maxProgressTreeMultiplierSliderDesc".Translate());
            maxProgressMultiplier = (float)Math.Round(listing_Standard.Slider(maxProgressMultiplier, 0.1f, 2f), 2);
            listing_Standard.Gap(25f);

            listing_Standard.Label("FoxianNineTailAgeTitle".Translate());
            listing_Standard.Gap(-4f);
            listing_Standard.GapLine();
            listing_Standard.Gap(5f);

            listing_Standard.Label("FoxianTailAgeInfo".Translate());
            listing_Standard.Gap(20f);

            listing_Standard.Label("FoxianOneTailAge".Translate(), tooltip: "FoxianOneTailAgeDesc".Translate());
            listing_Standard.TextFieldNumeric(ref oneTailAgeValue, ref oneTailAgeValueString, 0f, 100f);
            listing_Standard.Gap(10f);

            listing_Standard.Label("FoxianThreeTailAge".Translate(), tooltip: "FoxianThreeTailAgeDesc".Translate());
            listing_Standard.TextFieldNumeric(ref threeTailAgeValue, ref threeTailAgeValueString, 0f, 100f);
            listing_Standard.Gap(10f);

            listing_Standard.Label("FoxianFiveTailAge".Translate(), tooltip: "FoxianFiveTailAgeDesc".Translate());
            listing_Standard.TextFieldNumeric(ref fiveTailAgeValue, ref fiveTailAgeValueString, 0f, 100f);
            listing_Standard.Gap(10f);

            listing_Standard.Label("FoxianSevenTailAge".Translate(), tooltip: "FoxianSevenTailAgeDesc".Translate());
            listing_Standard.TextFieldNumeric(ref sevenTailAgeValue, ref sevenTailAgeValueString, 0f, 100f);
            listing_Standard.Gap(10f);

            listing_Standard.Label("FoxianNineTailAge".Translate(), tooltip: "FoxianNineTailAgeDesc".Translate());
            listing_Standard.TextFieldNumeric(ref nineTailAgeValue, ref nineTailAgeValueString, 0f, 100f);
            listing_Standard.Gap(25f);

            if(listing_Standard.ButtonText("ResetButton".Translate())) {
                SetDefaultValue();
            }
            Log.Message($"[FoxianMod] Settings content height: {listing_Standard.CurHeight}");
            listing_Standard.End();

            Widgets.EndScrollView();
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

        private static void ValidateTailsAgeValues() {
            if(sevenTailAgeValue > nineTailAgeValue) {
                sevenTailAgeValue = nineTailAgeValue - 1f;
                sevenTailAgeValueString = sevenTailAgeValue.ToString();
            }
            if(fiveTailAgeValue > sevenTailAgeValue) {
                fiveTailAgeValue = sevenTailAgeValue - 1f;
                fiveTailAgeValueString = fiveTailAgeValue.ToString();
            }
            if(threeTailAgeValue > fiveTailAgeValue) {
                threeTailAgeValue = fiveTailAgeValue - 1f;
                threeTailAgeValueString = threeTailAgeValue.ToString();
            }
        }

        private static void UpdateTailAgeValueStrings() {
            oneTailAgeValueString = oneTailAgeValue.ToString();
            threeTailAgeValueString = threeTailAgeValue.ToString();
            fiveTailAgeValueString = fiveTailAgeValue.ToString();
            sevenTailAgeValueString = sevenTailAgeValue.ToString();
            nineTailAgeValueString = nineTailAgeValue.ToString();
        }

        public static bool HasTailAgeValuesChanged() {
            return oneTailAgeValue != TailAgeValuesReference[0] ||
                   threeTailAgeValue != TailAgeValuesReference[1] ||
                   fiveTailAgeValue != TailAgeValuesReference[2] ||
                   sevenTailAgeValue != TailAgeValuesReference[3] ||
                   nineTailAgeValue != TailAgeValuesReference[4];
        }

        public static List<float> GetNewTailAgeValues() {
            return new List<float> { oneTailAgeValue, threeTailAgeValue, fiveTailAgeValue, sevenTailAgeValue, nineTailAgeValue };
        }
    }
}
