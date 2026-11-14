using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using static UnityEngine.GraphicsBuffer;

namespace FoxianPsycast {
    public class RitualOutcomeEffectWorker_SacrificialTree: RitualOutcomeEffectWorker_FromQuality {
        public static readonly SimpleCurve PercentageFromQuality = new SimpleCurve {
            new CurvePoint(0.2f, 0.4f),
            new CurvePoint(0.4f, 0.6f),
            new CurvePoint(0.6f, 0.8f),
            new CurvePoint(0.8f, 0.9f),
            new CurvePoint(1f, 1f)
        };

        public static readonly SimpleCurve QualityDuration = new SimpleCurve {
            new CurvePoint(0f, 1.1f),
            new CurvePoint(0.25f, 1.5f),
            new CurvePoint(0.32f, 2f),
            new CurvePoint(0.5f, 3f),
            new CurvePoint(0.62f, 4f),
            new CurvePoint(0.75f, 4f),
            new CurvePoint(1f, 6f)
        };

        public static readonly List<QualityCategory> AvailabelQualityCategories = new List<QualityCategory> { 
            QualityCategory.Normal, 
            QualityCategory.Good, 
            QualityCategory.Excellent 
        };

        private int minTimeBase = 20000;
        private int maxTimeBase = 200000;

        public override bool SupportsAttachableOutcomeEffect => false;

        public RitualOutcomeEffectWorker_SacrificialTree() {
        }

        public RitualOutcomeEffectWorker_SacrificialTree(RitualOutcomeEffectDef def)
            : base(def) {
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual) {
            float quality = GetQuality(jobRitual, progress);
            Pawn pawn = jobRitual.PawnWithRole("organizer");
            float outcomePercentage = PercentageFromQuality.Evaluate(quality);
            EventTypeOutcome(outcomePercentage, pawn, quality, progress, jobRitual);
        }

        private void EventTypeOutcome(float outcomePercentage, Pawn pawn, float quality, float progress, LordJob_Ritual jobRitual) {
            if (outcomePercentage == 1f) {
                //Automatic good
                if(ExcelentOutcome(outcomePercentage)) {
                    ThingDef thingReward = RewardSacrificialRitual.RareItems.RandomElement();
                    Thing thingItemReward;
                    GenSpawn.TrySpawn(thingReward, jobRitual.selectedTarget.Cell, jobRitual.Map, out thingItemReward);

                    if(thingItemReward.TryGetComp<CompQuality>(out CompQuality compQuality)) {
                        compQuality.SetQuality(AvailabelQualityCategories.RandomElement(), ArtGenerationContext.Colony);
                    }
                    CreateLetter("LetterLabelSacrificialRitualSatisfyingCompleted", "LetterTextSacrificialRitualSatisfyingCompleted", RewardType.BlessedItem, LetterDefOf.RitualOutcomePositive, pawn, quality, progress, jobRitual, thingItem: thingItemReward);
                }
                else {
                    HediffDef hediffReward = RewardSacrificialRitual.GoodHediffs.RandomElement();
                    if(IsDummyHediff(hediffReward)) {
                        hediffReward = GetCorrespondingWarriorHediff(pawn);
                    }

                    Hediff hediff = HediffMaker.MakeHediff(hediffReward, pawn, pawn.health.hediffSet.GetBrain());
                    HediffComp_Disappears hediffCompDisapear = hediff.TryGetComp<HediffComp_Disappears>();
                    if(hediffCompDisapear != null) hediffCompDisapear.ticksToDisappear = GetRandomTicksToDisapear(quality);
                    pawn.health.AddHediff(hediff);

                    CreateLetter("LetterLabelSacrificialRitualSuccessCompleted", "LetterTextSacrificialRitualSuccessCompleted", RewardType.GoodHediff, LetterDefOf.RitualOutcomePositive, pawn, quality, progress, jobRitual, hediffDef: hediffReward);
                }
            }
            else {
                float eventTypeValue = Rand.Value;
                if(eventTypeValue <= outcomePercentage) {
                    //Good outcome
                    if(ExcelentOutcome(outcomePercentage)) {
                        ThingDef thingReward = RewardSacrificialRitual.RareItems.RandomElement();
                        Thing thingItemReward;
                        GenSpawn.TrySpawn(thingReward, jobRitual.selectedTarget.Cell, jobRitual.Map, out thingItemReward);

                        if(thingItemReward.TryGetComp<CompQuality>(out CompQuality compQuality)) {
                            compQuality.SetQuality(AvailabelQualityCategories.RandomElement(), ArtGenerationContext.Colony);
                        }
                        CreateLetter("LetterLabelSacrificialRitualSatisfyingCompleted", "LetterTextSacrificialRitualSatisfyingCompleted", RewardType.BlessedItem, LetterDefOf.RitualOutcomePositive, pawn, quality, progress, jobRitual, thingItem: thingItemReward);
                    }
                    else {
                        HediffDef hediffReward = RewardSacrificialRitual.GoodHediffs.RandomElement();
                        if(IsDummyHediff(hediffReward)) {
                            hediffReward = GetCorrespondingWarriorHediff(pawn);
                        }

                        Hediff hediff = HediffMaker.MakeHediff(hediffReward, pawn, pawn.health.hediffSet.GetBrain());
                        HediffComp_Disappears hediffCompDisapear = hediff.TryGetComp<HediffComp_Disappears>();
                        if(hediffCompDisapear != null) hediffCompDisapear.ticksToDisappear = GetRandomTicksToDisapear(quality);
                        pawn.health.AddHediff(hediff);

                        CreateLetter("LetterLabelSacrificialRitualSuccessCompleted", "LetterTextSacrificialRitualSuccessCompleted", RewardType.GoodHediff, LetterDefOf.RitualOutcomePositive, pawn, quality, progress, jobRitual, hediffDef: hediffReward);
                    }
                }
                else {
                    //Bad outcome
                    float badOutcomeType = Rand.Value;
                    if(badOutcomeType <= 0.15f) {
                        IncidentDef incidentToLaunch = RewardSacrificialRitual.Incidents.RandomElement();
                        incidentToLaunch.Worker.TryExecute(StorytellerUtility.DefaultParmsNow(incidentToLaunch.category, jobRitual.Map));

                        CreateLetter("LetterLabelSacrificialRitualOffensiveCompleted", "LetterTextSacrificialRitualOffensiveCompleted", RewardType.Incident, LetterDefOf.RitualOutcomeNegative, pawn, quality, progress, jobRitual, incidentDef: incidentToLaunch);
                    }
                    else {
                        HediffDef hediffReward = RewardSacrificialRitual.BadHediffs.RandomElement();
                        Hediff hediff = HediffMaker.MakeHediff(hediffReward, pawn, pawn.health.hediffSet.GetBrain());

                        HediffComp_Disappears hediffCompDisapear = hediff.TryGetComp<HediffComp_Disappears>();
                        if(hediffCompDisapear != null) hediffCompDisapear.ticksToDisappear = GetRandomTicksToDisapear(quality, isDebuff: true);
                        pawn.health.AddHediff(hediff);

                        CreateLetter("LetterLabelSacrificialRitualOffensiveCompleted", "LetterTextSacrificialRitualOffensiveCompleted", RewardType.BadHediff, LetterDefOf.RitualOutcomeNegative, pawn, quality, progress, jobRitual, hediffDef: hediffReward);
                    }
                }
            }
            jobRitual.selectedTarget.Thing.Destroy();
        }

        private bool ExcelentOutcome(float outcomePercentage) {
            if (outcomePercentage < 0.6f) return false;
            float excelentOutcomeChance = outcomePercentage - 0.5f;
            float randomValue = Rand.Value;
            if(randomValue <= excelentOutcomeChance) {
                return true;
            }
            return false;
        }

        //Unused method, in case RandomElement is not compatible with Multiplayer
        private HediffDef GetHediffDefReward(List<HediffDef> hediffDefs) {
            int totalHediffs = hediffDefs.Count;
            if(totalHediffs == 1) return hediffDefs[0];

            float randArray = (Rand.Value * ((totalHediffs) - 1f) + 1f);
            decimal roundedRandArray = Math.Round((decimal)randArray);
            int selectArray = Math.Clamp((int)roundedRandArray, 0, totalHediffs) - 1;
            return hediffDefs[selectArray];
        }

        private bool IsDummyHediff(HediffDef hediffDef) {
            return hediffDef == InternalDefOf.FL_DummyHediffWarior;
        }

        private HediffDef GetCorrespondingWarriorHediff(Pawn pawn) {
            if((pawn.equipment.Primary?.def?.IsRangedWeapon) ?? false) {
                return InternalDefOf.FL_BlessingCunning;
            }
            else if((pawn.equipment.Primary?.def?.IsMeleeWeapon) ?? false) {
                return InternalDefOf.FL_BlessingPower;
            }
            else {
                if(pawn.skills.GetSkill(SkillDefOf.Shooting).Level > pawn.skills.GetSkill(SkillDefOf.Melee).Level) {
                    return InternalDefOf.FL_BlessingCunning;
                }
                else if(pawn.skills.GetSkill(SkillDefOf.Melee).Level > pawn.skills.GetSkill(SkillDefOf.Shooting).Level) {
                    return InternalDefOf.FL_BlessingPower;
                }
                return InternalDefOf.FL_BlessingCunning;
            }
        }

        private int GetRandomTicksToDisapear(float quality, bool isDebuff = false) {
            float timeMult = QualityDuration.Evaluate(quality);
            if (isDebuff) timeMult = 6.15f - timeMult;
            int randomValue = Mathf.RoundToInt(Rand.Value * (maxTimeBase - minTimeBase) + minTimeBase);
            return Mathf.RoundToInt(randomValue * timeMult);
        }

        private void CreateLetter(string labelLetter, string textLetter, RewardType rewardType, LetterDef letterDef, Pawn pawn, float quality, float progress, LordJob_Ritual jobRitual, HediffDef hediffDef = null, IncidentDef incidentDef = null, Thing thingItem = null) {
            string text = textLetter.Translate(pawn);
            string rewardTypeText = RewardTypeTextLetter(rewardType, pawn, hediffDef, incidentDef, thingItem);
            text = text + rewardTypeText;
            text = text + "\n\n" + OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            if (rewardType == RewardType.BlessedItem) Find.LetterStack.ReceiveLetter(labelLetter.Translate(), text, letterDef, new LookTargets(thingItem));
            else Find.LetterStack.ReceiveLetter(labelLetter.Translate(), text, letterDef, new LookTargets(pawn));
        }

        private string RewardTypeTextLetter(RewardType rewardType, Pawn pawn, HediffDef hediffDef, IncidentDef incidentDef, Thing thingItem) {
            switch (rewardType) {
                case RewardType.GoodHediff:
                    return "\n\n" + "SacrificialRitualReceiveBlessing".Translate(pawn, hediffDef);
                case RewardType.BadHediff:
                    return "\n\n" + "SacrificialRitualReceiveCurse".Translate(pawn, hediffDef);
                case RewardType.Incident:
                    return "\n\n" + "SacrificialRitualCauseIncident".Translate(pawn, incidentDef);
                case RewardType.BlessedItem:
                    return "\n\n" + "SacrificialRitualReceiveRareItem".Translate(pawn, thingItem.def);
                default:
                    return "";
            }
        }
    }
}
