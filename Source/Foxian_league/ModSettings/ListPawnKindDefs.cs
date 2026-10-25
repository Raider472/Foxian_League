using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foxian_league {
    //Class to store lists of PawnKindDefs for medieval mode
    public static class ListPawnKindDefs {
        //Foxian League side
        public static Dictionary<string, float> FoxianLeagueCombatPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Villager_Medieval", 2 },
            {"FL_Foxian_Town_Guard_Medieval", 10 },
            {"FL_Foxian_League_Grenadier_Flame_Medieval", 2.5f },
            {"FL_Foxian_League_Scout_Medieval", 6 },
            {"FL_Foxian_League_Infantryman_Medieval", 10 },
            {"FL_Foxian_League_Archer_Medieval", 5 },
            {"FL_Foxian_Man_At_Arms_Medieval", 6 },
            {"FL_Foxian_League_Hunter_Medieval", 4 },
            {"FL_Foxian_Veteran_Medieval", 5 },
            {"FL_Foxian_Officer_Medieval", 3 },
            {"FL_Foxian_Royal_Melee_Infantry_Medieval", 8 },
            {"FL_Foxian_Oracle_Medieval", 3 },
        };

        public static Dictionary<string, float> FoxianLeaguePeacefulPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Villager_Medieval", 20 },
            {"FL_Foxian_Villager_Child", 2 },
            {"FL_Foxian_Town_Guard_Medieval", 10 },
            {"FL_Foxian_Politician_Medieval", 10 },
            {"FL_Foxian_Noble_Medieval", 2 },
            {"FL_Foxian_Diplomat_Medieval", 4 },
        };

        public static Dictionary<string, Dictionary<string, float>> FoxianLeagueTraderPawnKindDefsList = new Dictionary<string, Dictionary<string, float>> {
            {"trader", new Dictionary<string, float> {
                {"FL_Foxian_Trader_Medieval", 1 }
            }},
            {"carriers", new Dictionary<string, float> {
                {"FL_Vulpano", 8},
                {"Muffalo", 2},
                {"Dromedary", 1},
                {"Alpaca", 1},
                {"Elephant", 1},
            }},
            {"guards", new Dictionary<string, float> {
                {"FL_Foxian_Diplomat_Medieval", 2 },
                {"FL_Foxian_Noble_Medieval", 2 },
                {"FL_Foxian_Villager_Medieval", 3 },
                {"FL_Foxian_Town_Guard_Medieval", 10 },
                {"FL_Foxian_League_Grenadier_Flame_Medieval", 1.5f },
                {"FL_Foxian_League_Infantryman_Medieval", 10 },
                {"FL_Foxian_League_Archer_Medieval", 10 },
                {"FL_Foxian_League_Hunter_Medieval", 6 },
                {"FL_Foxian_Officer_Medieval", 3 },
                {"FL_Foxian_Man_At_Arms_Medieval", 5 },
                {"FL_Foxian_Veteran_Medieval", 4 },
                {"FL_Foxian_Royal_Melee_Infantry_Medieval", 8 },
            }},
        };

        public static Dictionary<string, float> FoxianLeagueSettlementPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Villager_Medieval", 10 },
            {"FL_Foxian_Diplomat_Medieval", 4 },
            {"FL_Foxian_Villager_Child", 2 },
            {"FL_Foxian_Noble_Medieval", 4 },
            {"FL_Foxian_Politician_Medieval", 8 },
            {"FL_Foxian_Town_Guard_Medieval", 10 },
            {"FL_Foxian_League_Grenadier_Flame_Medieval", 2 },
            {"FL_Foxian_League_Infantryman_Medieval", 10 },
            {"FL_Foxian_League_Scout_Medieval", 5 },
            {"FL_Foxian_League_Archer_Medieval", 7 },
            {"FL_Foxian_League_Hunter_Medieval", 8 },
            {"FL_Foxian_Veteran_Medieval", 6 },
            {"FL_Foxian_Officer_Medieval", 3 },
            {"FL_Foxian_Royal_Melee_Infantry_Medieval", 8 },
            {"FL_Vulpano", 0.5f },
        };

        //Blood Rain
        public static Dictionary<string, float> BloodRainCombatPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Raider_Human_Slave_Medieval", 3.5f },
            {"FL_Foxian_Raider_Trash_Medieval", 7 },
            {"FL_Foxian_Raider_Scavenger_Melee_Medieval", 9 },
            {"FL_Foxian_Raider_kidnapper_Medieval", 2.5f },
            {"FL_Foxian_Raider_Pyromaniac_Medieval", 4.5f },
            {"FL_Foxian_Raider_Looter_Medieval", 8 },
            {"FL_Foxian_Raider_Grenadier_Medieval", 2.5f },
            {"FL_Foxian_Raider_Tracker_Medieval", 6.5f },
            {"FL_Foxian_Raider_Breacher_Medieval", 3 },
            {"FL_Raider_Foxian_Despoiler_Medieval", 7 },
            {"FL_Foxian_Raider_Local_Commander_Medieval", 1 },
            {"FL_Foxian_Raider_Shredder_Medieval", 5 },
            {"FL_Foxian_Raider_Children_Of_Blood_Medieval", 4 },
            {"FL_Vulyakko", 4 },
            {"FL_Hulkarash", 1 },
        };

        public static Dictionary<string, float> BloodRainCombatAnimalPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Raider_Human_Slave_Medieval", 3.5f },
            {"FL_Foxian_Raider_Trash_Medieval", 8 },
            {"FL_Foxian_Raider_Scavenger_Melee_Medieval", 9 },
            {"FL_Foxian_Raider_kidnapper_Medieval", 2 },
            {"FL_Foxian_Raider_Pyromaniac_Medieval", 3.5f },
            {"FL_Foxian_Raider_Looter_Medieval", 8 },
            {"FL_Foxian_Raider_Grenadier_Medieval", 2.5f },
            {"FL_Foxian_Raider_Tracker_Medieval", 6.5f },
            {"FL_Foxian_Raider_Breacher_Medieval", 3 },
            {"FL_Raider_Foxian_Despoiler_Medieval", 7 },
            {"FL_Foxian_Raider_Local_Commander_Medieval", 1 },
            {"FL_Foxian_Raider_Shredder_Medieval", 5 },
            {"FL_Foxian_Raider_Children_Of_Blood_Medieval", 4 },
            {"FL_Vulyakko", 6 },
            {"FL_Hulkarash", 3 },
        };

        public static Dictionary<string, float> BloodRainCombatTrashPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Raider_Human_Slave_Medieval", 5f },
            {"FL_Foxian_Raider_Trash_Medieval", 10 },
            {"FL_Foxian_Raider_Scavenger_Melee_Medieval", 3 },
            {"FL_Vulyakko", 5 },
            {"FL_Hulkarash", 1 },
        };

        public static Dictionary<string, Dictionary<string, float>> BloodRainTraderPawnKindDefsList = new Dictionary<string, Dictionary<string, float>> {
            {"trader", new Dictionary<string, float> {
                {"FL_RaiderFoxian_Trader_Medieval", 1 }
            }},
            {"carriers", new Dictionary<string, float> {
                {"FL_Hulkarash", 8},
            }},
            {"guards", new Dictionary<string, float> {
                {"FL_Foxian_Raider_Villager_Medieval", 3 },
                {"FL_Foxian_Raider_Scavenger_Melee_Medieval", 7 },
                {"FL_Foxian_Raider_Human_Slave_Medieval", 1.5f },
                {"FL_Foxian_Raider_Looter_Medieval", 10 },
                {"FL_Foxian_Raider_Tracker_Medieval", 8f },
                {"FL_Raider_Foxian_Despoiler_Medieval", 8 },
                {"FL_Foxian_Raider_Local_Commander_Medieval", 1 },
                {"FL_Foxian_Raider_Shredder_Medieval", 6 },
            }},
        };

        public static Dictionary<string, float> BloodRainSettlementPawnKindDefsList = new Dictionary<string, float> {
            {"FL_Foxian_Raider_Human_Slave_Medieval", 0.5f },
            {"FL_Foxian_Raider_Trash_Medieval", 4 },
            {"FL_Foxian_Raider_Scavenger_Melee_Medieval", 8 },
            {"FL_Foxian_Raider_Pyromaniac_Medieval", 3 },
            {"FL_Foxian_Raider_Looter_Medieval", 6 },
            {"FL_Foxian_Raider_Grenadier_Medieval", 2.5f },
            {"FL_Foxian_Raider_Tracker_Medieval", 8.5f },
            {"FL_Raider_Foxian_Despoiler_Medieval", 8 },
            {"FL_Foxian_Raider_Local_Commander_Medieval", 1 },
            {"FL_Foxian_Raider_Shredder_Medieval", 10 },
            {"FL_Foxian_Raider_Children_Of_Blood_Medieval", 2 },
            {"FL_Vulyakko", 2 },
            {"FL_Hulkarash", 1 },
        };

        //Foreign Settlement
        public static Dictionary<string, float> minerSettlement = new Dictionary<string, float> {
            {"Tribal_Miner", 1 },
        };

        public static Dictionary<string, float> HunterSettlement = new Dictionary<string, float> {
            {"Tribal_Hunter", 1 },
        };

        public static Dictionary<string, float> LoggerSettlement = new Dictionary<string, float> {
            {"Tribal_Logger", 1 },
        };

        public static Dictionary<string, float> FarmersSettlement = new Dictionary<string, float> {
            {"Tribal_Farmer", 1 },
        };
    }
}
