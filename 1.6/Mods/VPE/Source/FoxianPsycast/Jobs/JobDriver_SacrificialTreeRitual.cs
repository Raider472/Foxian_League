using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using static RimWorld.FleshTypeDef;
using Vector3 = UnityEngine.Vector3;

namespace FoxianPsycast {
    public class JobDriver_SacrificialTreeRitual: JobDriver {
        protected const TargetIndex SacrificialTreeInd = TargetIndex.A;
        protected const TargetIndex LinkSpotInd = TargetIndex.B;
        private Thing SacrificialTreeThing => base.TargetA.Thing;
        private LocalTargetInfo LinkSpot => job.targetB;

        public override bool TryMakePreToilReservations(bool errorOnFailed) {
            if(pawn.Reserve(SacrificialTreeThing, job, 1, -1, null, errorOnFailed)) {
                return pawn.Reserve(LinkSpot, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils() {
            if (SacrificialTreeThing == null || LinkSpot == null) yield break;
            this.FailOnDespawnedOrNull(SacrificialTreeInd);
            this.FailOnDespawnedOrNull(LinkSpotInd);
            yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
            Toil toil = Toils_General.Wait(10000);
            toil.tickIntervalAction = delegate (int delta) {
                pawn.rotationTracker.FaceTarget(SacrificialTreeThing);
                if(pawn.IsHashIntervalTick(720, delta)) {
                    Vector3 vector = pawn.TrueCenter();
                    vector += (SacrificialTreeThing.TrueCenter() - vector) * Rand.Value;
                    FleckMaker.Static(vector, pawn.Map, FleckDefOf.PsycastAreaEffect, 0.5f);
                    SoundDefOf.PsycastPsychicEffect.PlayOneShot(SoundInfo.InMap(new TargetInfo(SacrificialTreeThing)));
                }
            };
            toil.handlingFacing = false;
            toil.socialMode = RandomSocialMode.Off;
            yield return toil;
        }
    }
}
