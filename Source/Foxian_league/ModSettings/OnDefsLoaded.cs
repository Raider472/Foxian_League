using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    // This static constructor will be called when the game loads the defs and will apply the settings to the defs.
    [StaticConstructorOnStartup]
    public class OnDefsLoaded {
        static OnDefsLoaded() {
            ApplySettingsToDefs();
        }

        public static void ApplySettingsToDefs() {
            if(!Foxian_Settings.IsFoxianBi) {
                Log.Message("Foxian League: IsFoxianBi is false, removing forcedTraits from FL_Lustful");
                InternalDefOf.FL_Lustful.forcedTraits = null;
            }
            if(Foxian_Settings.medievalMode) {
                Log.Message("Foxian League: Medieval mode is enabled");
                prepareMedievalMode();
            }
            if(Foxian_Settings.HasTailAgeValuesChanged()) {
                //Reflection because the .dll for better gene graphics is not on .NET 4.7.2
                Log.Message("[Foxian League]: Tail age values have changed, updating tail defs.");
                List<float> newTailAgeValues = Foxian_Settings.GetNewTailAgeValues();

                Type ageWithGraphicsModExtensionType = Type.GetType("BetterGeneGraphicsFramework.GraphicsWithAge, BetterGeneGraphicsFramework");
                if(ageWithGraphicsModExtensionType == null) {
                    Log.Warning("AgeWithGraphics ModExtension Type not found.");
                    return;
                }

                object GraphicsAgeInstance = InternalDefOf.FL_NineFoxTail.modExtensions.FirstOrDefault(e => ageWithGraphicsModExtensionType.IsInstanceOfType(e));
                if(GraphicsAgeInstance == null) {
                    Log.Warning("No GraphicsWithAge instance found on FL_NineFoxTail.");
                    return;
                }

                FieldInfo agesField = ageWithGraphicsModExtensionType.GetField("ages");
                List<float> TailValueAges = agesField.GetValue(GraphicsAgeInstance) as List<float>;
                TailValueAges.Clear();
                TailValueAges.AddRange(newTailAgeValues);
            }
        }

        //Medieval Mode Only
        //Method to Prepare the pawnKindDefs List and etc. for medieval mode (Not a patch to medieval overhaul)
        private static void prepareMedievalMode() {
            Log.Message("Preparing medieval mode settings...");
            FactionDef foxianLeagueDef = DefDatabase<FactionDef>.GetNamed("Foxian_League");
            CreateFoxianLeagueList(foxianLeagueDef);
            foxianLeagueDef.techLevel = TechLevel.Medieval;
            foxianLeagueDef.canSiege = false;
            CreateFixedLeaderKinds(foxianLeagueDef, "FL_Foxian_League_counselor_Medieval");
            ReplaceTraderKind(foxianLeagueDef);

            FactionDef bloodRainDef = DefDatabase<FactionDef>.GetNamed("Foxian_BloodRain");
            CreateBloodRainList(bloodRainDef);
            bloodRainDef.techLevel = TechLevel.Medieval;
            bloodRainDef.canSiege = false;
            CreateFixedLeaderKinds(bloodRainDef, "FL_Foxian_Raider_Children_Of_Blood_Medieval");
            ReplaceTraderKind(bloodRainDef);

            ScenarioDef foxianRefugeeScenario = DefDatabase<ScenarioDef>.GetNamedSilentFail("FL_Foxian_Scenario_Refugees");
            ChangeScenPartWeapons(foxianRefugeeScenario, "Gun_BoltActionRifle", "Gun_Autopistol");

            ScenarioDef foxianBloodRainExileeScenario = DefDatabase<ScenarioDef>.GetNamedSilentFail("FL_Foxian_BloodRain_Scenario_Refugees");
            ChangeScenPartWeapons(foxianBloodRainExileeScenario, "FL_Gun_HarpoonGun", "Gun_Autopistol", "MeleeWeapon_LongSword", "MeleeWeapon_Spear");

            FactionDef foxianPlayerColonyDef = DefDatabase<FactionDef>.GetNamed("FoxianPlayerColony");
            foxianPlayerColonyDef.techLevel = TechLevel.Medieval;
            FactionDef foxianPlayerBloodRainDef = DefDatabase<FactionDef>.GetNamed("FoxianBloodRainPlayerColony");
            foxianPlayerBloodRainDef.techLevel = TechLevel.Medieval;
        }

        private static void CreateFoxianLeagueList(FactionDef factionDef) {
            List<PawnGroupMaker> pawnGroupMakers = new List<PawnGroupMaker>();
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.FoxianLeagueCombatPawnKindDefsList, "Combat"));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.FoxianLeaguePeacefulPawnKindDefsList, "Peaceful"));
            pawnGroupMakers.Add(CreatePawnGroupMakerTrader(ListPawnKindDefs.FoxianLeagueTraderPawnKindDefsList));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.FoxianLeagueSettlementPawnKindDefsList, "Settlement"));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.minerSettlement, "Miners", 1));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.HunterSettlement, "Hunters", 1));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.LoggerSettlement, "Loggers", 1));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.FarmersSettlement, "Farmers", 1));
            factionDef.pawnGroupMakers.Clear();
            factionDef.pawnGroupMakers.AddRange(pawnGroupMakers);
            Log.Message($"Created Foxian League pawn group makers for faction {factionDef.defName} with {pawnGroupMakers.Count} group makers.");
        }

        private static void CreateBloodRainList(FactionDef factionDef) {
            List<PawnGroupMaker> pawnGroupMakers = new List<PawnGroupMaker>();
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.BloodRainCombatPawnKindDefsList, "Combat"));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.BloodRainCombatAnimalPawnKindDefsList, "Combat", 30f));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.BloodRainCombatTrashPawnKindDefsList, "Combat", 2f));
            pawnGroupMakers.Add(CreatePawnGroupMakerTrader(ListPawnKindDefs.BloodRainTraderPawnKindDefsList));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.BloodRainSettlementPawnKindDefsList, "Settlement"));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.minerSettlement, "Miners", 1));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.HunterSettlement, "Hunters", 1));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.LoggerSettlement, "Loggers", 1));
            pawnGroupMakers.Add(CreatePawnGroupMaker(ListPawnKindDefs.FarmersSettlement, "Farmers", 1));
            factionDef.pawnGroupMakers.Clear();
            factionDef.pawnGroupMakers.AddRange(pawnGroupMakers);
            Log.Message($"Created Blood Rain pawn group makers for faction {factionDef.defName} with {pawnGroupMakers.Count} group makers.");
        }

        private static PawnGroupMaker CreatePawnGroupMaker(Dictionary<string, float> pawnKindOption, string kindDefName, float commonality = 100f) {
            PawnGroupMaker pawnGroupMaker = new PawnGroupMaker {
                commonality = commonality,
                kindDef = DefDatabase<PawnGroupKindDef>.GetNamed(kindDefName),
                options = CreatePawnGenOptionList(pawnKindOption)
            };
            Log.Message($"Created PawnGroupMaker for {kindDefName} with {pawnGroupMaker.options.Count} options and commonality {commonality}.");
            return pawnGroupMaker;
        }

        private static PawnGroupMaker CreatePawnGroupMakerTrader(Dictionary<string, Dictionary<string, float>> pawnKindOption) {
            PawnGroupMaker pawnGroupMaker = new PawnGroupMaker {
                kindDef = DefDatabase<PawnGroupKindDef>.GetNamed("Trader"),
                traders = CreatePawnGenOptionList(pawnKindOption["trader"]),
                carriers = CreatePawnGenOptionList(pawnKindOption["carriers"]),
                guards = CreatePawnGenOptionList(pawnKindOption["guards"])
            };
            Log.Message($"Created PawnGroupMaker for Trader with {pawnGroupMaker.traders.Count} traders, {pawnGroupMaker.carriers.Count} carriers, and {pawnGroupMaker.guards.Count} guards.");
            return pawnGroupMaker;
        }

        private static List<PawnGenOption> CreatePawnGenOptionList(Dictionary<string, float> pawnKindOptionDict) {
            List<PawnGenOption> pawnGenOptions = new List<PawnGenOption>();

            foreach (var item in pawnKindOptionDict) {
                PawnGenOption pawnGenOptionCreate = new PawnGenOption {
                    kind = DefDatabase<PawnKindDef>.GetNamed(item.Key),
                    selectionWeight = item.Value
                };
                pawnGenOptions.Add(pawnGenOptionCreate);
            }
            Log.Message($"Created {pawnGenOptions.Count} PawnGenOptions for {pawnKindOptionDict.Keys.FirstOrDefault()}");
            return pawnGenOptions;
        }

        private static void CreateFixedLeaderKinds(FactionDef factionDef, string leaderKindName) {
            List<PawnKindDef> LeaderKindList = new List<PawnKindDef> {
                DefDatabase<PawnKindDef>.GetNamed(leaderKindName),
            };
            factionDef.fixedLeaderKinds.Clear();
            factionDef.fixedLeaderKinds.AddRange(LeaderKindList);
        }

        private static void ReplaceTraderKind(FactionDef factionDef) {
            List<TraderKindDef> traderKindsCaravan = new List<TraderKindDef> {
                DefDatabase<TraderKindDef>.GetNamed("Caravan_Neolithic_BulkGoods"),
                DefDatabase<TraderKindDef>.GetNamed("Caravan_Neolithic_WarMerchant"),
                DefDatabase<TraderKindDef>.GetNamed("Caravan_Neolithic_Slaver"),
                DefDatabase<TraderKindDef>.GetNamed("Caravan_Neolithic_ShamanMerchant"),
            };
            factionDef.caravanTraderKinds.Clear();
            factionDef.caravanTraderKinds.AddRange(traderKindsCaravan);

            List<TraderKindDef> traderKindsVisitor = new List<TraderKindDef> {
                DefDatabase<TraderKindDef>.GetNamed("Visitor_Neolithic_Standard"),
            };
            factionDef.visitorTraderKinds.Clear();
            factionDef.visitorTraderKinds.AddRange(traderKindsVisitor);

            List<TraderKindDef> traderKindsBase = new List<TraderKindDef> {
                DefDatabase<TraderKindDef>.GetNamed("Base_Neolithic_Standard"),
            };
            factionDef.baseTraderKinds.Clear();
            factionDef.baseTraderKinds.AddRange(traderKindsBase);
        }

        private static void ChangeScenPartWeapons(ScenarioDef scenarioDef, string firstWeaponName, string secondWeaponName, string firstWeaponReplacement = "MeleeWeapon_Spear", string secondWeaponReplacement = "MeleeWeapon_Mace", string stuff = "Steel") {
            if(scenarioDef != null) {
                foreach(ScenPart part in scenarioDef.scenario.AllParts) {
                    Log.Message($"Scenario part: {part.GetType().Name}");
                    if(part is ScenPart_StartingThing_Defined_FoxianExtended startingThing && startingThing.ThingDef == DefDatabase<ThingDef>.GetNamedSilentFail(firstWeaponName)) {
                        startingThing.SetThingDef(DefDatabase<ThingDef>.GetNamedSilentFail(firstWeaponReplacement));
                        startingThing.setStuffDef(DefDatabase<ThingDef>.GetNamedSilentFail(stuff));
                        Log.Message($"Replaced {firstWeaponName} with {firstWeaponReplacement} in FL_Foxian_Scenario_Refugees scenario.");
                    }
                    if(part is ScenPart_StartingThing_Defined_FoxianExtended startingThing2 && startingThing2.ThingDef == DefDatabase<ThingDef>.GetNamedSilentFail(secondWeaponName)) {
                        startingThing2.SetThingDef(DefDatabase<ThingDef>.GetNamedSilentFail(secondWeaponReplacement));
                        startingThing2.setStuffDef(DefDatabase<ThingDef>.GetNamedSilentFail(stuff));
                        Log.Message($"Replaced {secondWeaponName} with {secondWeaponReplacement} in FL_Foxian_Scenario_Refugees scenario.");
                    }
                }
            }
        }
    }
}
