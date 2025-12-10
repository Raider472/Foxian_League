using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class Comp_CursedSwordBehaviour : ThingComp {
        //Comp to add and hediff when equipped and to make the "Insatiable hunger" work
        public ComProperties_CursedSwordBehaviour Props => (ComProperties_CursedSwordBehaviour)props;

        public Dictionary<string, float> severityDict = new Dictionary<string, float>() {
            { "minShambler", 0.03f },
            { "maxShambler", 0.11f },
            { "minEntity", 0.08f },
            { "maxEntity", 0.31f },
            { "minHuman", 0.18f },
            { "maxHuman", 0.42f },
            { "minAnimal", 0.04f },
            { "maxAnimal", 0.19f }
        };

        public override void Notify_Equipped(Pawn pawn) {
            if (pawn == null || Props.hediffOnEquip == null) return;
            Log.Message($"Pawn: {pawn} will receive the effect");
            base.Notify_Equipped(pawn);
            if (pawn.health.hediffSet.TryGetHediff(Props.hediffOnEquip, out Hediff hediff)) {
                Log.Message("This pawn already has the hediff");
                return;
            }
            Hediff hediffToAdd = HediffMaker.MakeHediff(Props.hediffOnEquip, pawn, null);
            if(!pawn.Faction.IsPlayer) {
                float severity = (Rand.Value * (0.64f - 0.04f) + 0.04f);
                hediffToAdd.Severity = severity;
            }
            Log.Message($"who is pawn: {pawn}, is pawn faction leader ? {PawnUtility.IsFactionLeader(pawn)}, is props alt there? {Props.hediffOnEquipAlt}");
            if(pawn.Faction.leader == null && !pawn.Faction.IsPlayer && Props.hediffOnEquipAlt != null) {
                Log.Message($"pawn is a non player faction leader: {pawn}");
                hediffToAdd = HediffMaker.MakeHediff(Props.hediffOnEquipAlt, pawn, null);
            }
            pawn.health.AddHediff(hediffToAdd);
        }

        public override void Notify_Unequipped(Pawn pawn) {
            if (pawn == null || pawn.Dead) return;
            base.Notify_Unequipped(pawn);
            if (pawn.health.InPainShock) {
                Log.Message("Help this pawn plssssssss");
                pawn.needs.mood.thoughts.memories.TryGainMemoryFast(InternalDefOf.FL_CursedSwordRemoved, 0);
            }
            else {
                Log.Message("He did this on his own");
                if (Rand.Chance(0.49f)) {
                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, "CursedSwordBerseker".Translate(pawn), true);
                }
                else {
                    if (Rand.Chance(0.37f)) {
                        pawn.needs.mood.thoughts.memories.TryGainMemoryFast(InternalDefOf.FL_CursedSwordRemoved, 1);
                    }
                    else {
                        Log.Message("Comma time");
                        Hediff comaHediff = HediffMaker.MakeHediff(InternalDefOf.FL_PsychicComaRandom, pawn, null);
                        pawn.health.AddHediff(comaHediff);
                    }
                }
            }
            if (pawn.health.hediffSet.TryGetHediff(Props.hediffOnEquip, out Hediff hediffRef)) {
                pawn.health.RemoveHediff(hediffRef);
            }
            else if(pawn.health.hediffSet.TryGetHediff(Props.hediffOnEquipAlt, out Hediff hediffRefAlt)) {
                pawn.health.RemoveHediff(hediffRefAlt);
            }
        }

        public override void Notify_KilledPawn(Pawn pawn) {
            //Pawn is the pawn who killed
            if (pawn.GetType() != typeof(Pawn)) return;
            base.Notify_KilledPawn(pawn);
            LocalTargetInfo latestTarget = pawn.LastAttackedTarget;
            Log.Message($"The killed Pawn is: {latestTarget}, and here is the type {pawn.GetType()}");
            float severityToApply = CalculateSeverity(latestTarget);
            Log.Message($"Severity that will be added: {severityToApply}");
            if(pawn.health.hediffSet.TryGetHediff(Props.hediffOnEquip, out Hediff hediffRef)) {
                hediffRef.Severity -= severityToApply;
            }
            return;
        }

        public float CalculateSeverity(LocalTargetInfo latestTarget) {
            if (latestTarget == null) return 0f;

            string pawnCategory = GetPawnCategory(latestTarget.Pawn);
            Log.Message($"Catgeory is: {pawnCategory}");
            if (pawnCategory == null) return 0f;

            float minimum = severityDict.GetValueOrDefault("min" + pawnCategory, 0f);
            float maximum = severityDict.GetValueOrDefault("max" + pawnCategory, 0f);
            return (Rand.Value * (maximum - minimum) + minimum);
        }

        private string GetPawnCategory(Pawn pawn) {
            if (pawn.IsShambler) return "Shambler";
            if (pawn.IsEntity) return "Entity";
            if (pawn.RaceProps?.Humanlike == true) return "Human";
            if (pawn.IsAnimal) return "Animal";
            return null;
        }
    }
}
