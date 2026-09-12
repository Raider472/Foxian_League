using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Patch to give a taming boost to pawn having the Animal Connection gene when trying to recruit an animal
    //The taming boost is based on the tamer's psychic sensitivity, with a curve defined in the tamingIncreaseCurve variable
    //The Patch was made with the help of ChatGPT Because I am really struggling with Transpilers, the code is a mess and that's on purpose.
    //I want to have a concrete example if I ever touch Transpilers again
    [HarmonyPatch(typeof(InteractionWorker_RecruitAttempt))]
    public static class Patch_InteractionWorker_RecruitAttempt {
        private static readonly SimpleCurve tamingIncreaseCurve = new SimpleCurve {
            new CurvePoint(1f, 0.05f),
            new CurvePoint(1.5f, 0.1f),
            new CurvePoint(2f, 0.15f),
            new CurvePoint(2.5f, 0.2f),
            new CurvePoint(3f, 0.25f),
        };

        static MethodBase TargetMethod() {
            return AccessTools.Method(
                typeof(InteractionWorker_RecruitAttempt),
                "Interacted",
                new Type[] {
                    typeof(Pawn),
                    typeof(Pawn),
                    typeof(List<RulePackDef>),
                    typeof(string).MakeByRefType(),
                    typeof(string).MakeByRefType(),
                    typeof(LetterDef).MakeByRefType(),
                    typeof(LookTargets).MakeByRefType()
                }
            );
        }

        private static void InjectTamingIncrease(Pawn tamer, ref float num2) {
            if(Utils.HasActiveGene(tamer, InternalDefOf.FL_AnimalConnection)) {
                Log.Message($"Debug Injecting taming increase for {tamer.Name}, Psychic Sensitivity: {tamer.GetStatValue(StatDefOf.PsychicSensitivity)}, Taming Increase: {tamingIncreaseCurve.Evaluate(tamer.GetStatValue(StatDefOf.PsychicSensitivity))}, num2: {num2}");
                num2 += tamingIncreaseCurve.Evaluate(
                    tamer.GetStatValue(StatDefOf.PsychicSensitivity)
                );
                Log.Message($"AFTER: {num2}");
            }
        }

        //After Wildness Modifier
        /*[HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);
            FieldInfo wildnessCurve = AccessTools.Field(typeof(InteractionWorker_RecruitAttempt), "TameChanceFactorCurve_Wildness");
            MethodInfo injectTamingIncrease = AccessTools.Method(typeof(Patch_InteractionWorker_RecruitAttempt), "InjectTamingIncrease", new Type[] { typeof(Pawn), typeof(float).MakeByRefType() });

            codeMatcher.MatchStartForward(
                    CodeMatch.Calls(
                        AccessTools.Method(
                            typeof(SimpleCurve),
                            nameof(SimpleCurve.Evaluate),
                            new[] { typeof(float) }
                        )
                    )
                )
                .ThrowIfNotMatchForward("Could not find specific line - Foxian Transpiler InteractionWorker_RecruitAttempt")
                .Advance(3)
                .Insert(
                    new CodeInstruction(OpCodes.Ldarg_1), // Load the tamer pawn (second argument)
                    new CodeInstruction(OpCodes.Ldloca_S, (byte)3), // Load the address of the local variable (float result)
                    new CodeInstruction(OpCodes.Call, injectTamingIncrease) // Call the InjectTamingIncrease method
                );

            Log.Message("========== AFTER INJECTION ==========");

            for(int i = 0; i < codeMatcher.Length; i++) {
                Log.Message($"{i:D3}: {codeMatcher.Instructions()[i]}");
            }

            int injectionPosition = codeMatcher.Pos;
            Log.Message($"Injection position: {injectionPosition}");

            return codeMatcher.Instructions();
        }*/

        //Before Wildness Modifier
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);
            FieldInfo tameAnimalChance = AccessTools.Field(typeof(StatDefOf), nameof(StatDefOf.TameAnimalChance));
            MethodInfo injectTamingIncrease = AccessTools.Method(typeof(Patch_InteractionWorker_RecruitAttempt), nameof(InjectTamingIncrease), new Type[] { typeof(Pawn), typeof(float).MakeByRefType() });

            codeMatcher
            .MatchStartForward(
                new CodeMatch(
                    OpCodes.Ldsfld,
                    tameAnimalChance
                )
            )
            .ThrowIfNotMatchForward(
                "Could not find TameAnimalChance - Foxian Transpiler"
            )
            .Advance(5)
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldloca_S, (byte)3),
                new CodeInstruction(OpCodes.Call, injectTamingIncrease)
            );

            /*Log.Message("========== AFTER INJECTION ==========");

            for(int i = 0; i < codeMatcher.Length; i++) {
                Log.Message($"{i:D3}: {codeMatcher.Instructions()[i]}");
            }*/

            int injectionPosition = codeMatcher.Pos;
            Log.Message($"Injection position: {injectionPosition}");

            return codeMatcher.Instructions();
        }
    }
}
