using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEF.Abilities;
using VEF.Weapons;
using Verse;

namespace FoxianPsycast{
    //Similar to VEF Ability Spawn, Did this because Spawn did not work with PawnKindDef
    public class FL_Ability_SpawnTreant : VEF.Abilities.Ability {

        public static readonly SimpleCurve DurationFromPsychicSentivity = new SimpleCurve {
            new CurvePoint(0.5f, 60000f),
            new CurvePoint(1f, 150000f),
            new CurvePoint(1.5f, 270000f),
            new CurvePoint(2f, 360000f),
            new CurvePoint(2.5f, 480000f),
            new CurvePoint(3f, 600000f),
            new CurvePoint(4f, 900000f),
            new CurvePoint(5f, 1000000f)
        };
        public override bool CanAutoCast => false;

        public override void Cast(params GlobalTargetInfo[] targets) {
            base.Cast(targets);
            AbilityExtension_SpawnPawn modExtension = def.GetModExtension<AbilityExtension_SpawnPawn>();

            if(modExtension?.pawn != null) {
                for(int i = 0; i < targets.Length; i++) {
                    Log.Message($"Spawning {modExtension.pawn.defName} at {targets[i].Cell}");
                    Spawn(targets[i], modExtension.pawn, this);
                }
            }
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true) {
            AbilityExtension_SpawnPawn modExtension = def.GetModExtension<AbilityExtension_SpawnPawn>();

            if(modExtension == null || modExtension?.pawn == null) return false;

            Thing treePlant = target.Cell.GetPlant(pawn.Map);

            if(treePlant == null || treePlant.def.plant.wildOrder < 3) {
                if(showMessages) {
                    Messages.Message("AbilityTargetMustTargetTree".Translate(), target.ToTargetInfo(pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }

            return base.ValidateTarget(target, showMessages);
        }

        public void Spawn(GlobalTargetInfo target, PawnKindDef pawnKind, VEF.Abilities.Ability ability) {
            Plant treePlant = target.Cell.GetPlant(ability.pawn.Map);
            treePlant.Kill();

            Pawn pawn = PawnGenerator.GeneratePawn(pawnKind);
            Log.Message($"Spawned {pawnKind.defName} at {target.Cell}");
            Thing thing = GenSpawn.Spawn(pawn, target.Cell, target.Map);
            string generated = NameGenerator.GenerateName(pawn.RaceProps.GetNameGenerator(pawn.gender));
            pawn.Name = new NameSingle(generated); ;


            thing.SetFaction(ability.pawn.Faction);
            //InteractionWorker_RecruitAttempt.DoRecruit(ability.pawn, pawn);

            //pawn.training.SetWantedRecursive(TrainableDefOf.Tameness, false);
            Log.Message($"No training found for {pawnKind.defName}, training all trainable defs");
            foreach(TrainableDef allDef in DefDatabase<TrainableDef>.AllDefs) {
                Log.Message($"Training {pawnKind.defName} in {allDef.defName}");
                if(pawn.training.CanAssignToTrain(allDef).Accepted) {
                    Log.Message($"Inside CanAssignToTrain:");
                    pawn.training.Train(allDef, null, complete: true);
                }
            }
            pawn.playerSettings.Master = ability.pawn;
            pawn.playerSettings.followDrafted = true;
            pawn.playerSettings.followFieldwork = true;

            Log.Message($"Count List: {pawn.health.hediffSet.hediffs.Count}");
            pawn.health.hediffSet.hediffs.Clear();

            int durationFromPsychicSensitivity = (int)DurationFromPsychicSentivity.Evaluate(ability.pawn.psychicEntropy.PsychicSensitivity);
            Log.Message($"Duration from Psychic Sensitivity: {durationFromPsychicSensitivity}");
            Hediff hediff = HediffMaker.MakeHediff(InternalDefOf.FL_PsychicalBody, pawn, pawn.health.hediffSet.GetBrain());
            HediffComp_Disappears hediffCompDisapear = hediff.TryGetComp<HediffComp_Disappears>();
            if(hediffCompDisapear != null) hediffCompDisapear.ticksToDisappear = durationFromPsychicSensitivity;
            pawn.health.AddHediff(hediff);
        }
    }
}
