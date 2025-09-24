using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Class use for patching the UI when the player hover a skill inside the skill tabs
    //This will show how much apptitudes the Psychic manipulation gene will give to the selected pawn
    //Use a Transpiller because the Postfix patching will create an out of place string
    [HarmonyPatch(typeof(SkillUI), "GetSkillDescription")]
    public static class Patch_SkillUI_GetSkillDescription {

        public static SkillRecord SkillRecord;

        [HarmonyPrefix]
        public static void PreFix(SkillRecord sk) {
            if(sk == null) return;
            if (SkillRecord != sk) {
                SkillRecord = sk;
            }
        }

        //This method is used so I don't have to program something in assembly
        private static void InjectStringSb(StringBuilder sb) {
            if (Utils.HasActiveGene(SkillRecord.Pawn, InternalDefOf.FL_PsychicManipulation) && SkillRecord.def == DefDatabase<SkillDef>.GetNamed(string.Concat(Foxian_Settings.skillEnum))) {
                Gene_PsychicManipulation psychicManip = SkillRecord.Pawn.genes.GetGene(InternalDefOf.FL_PsychicManipulation) as Gene_PsychicManipulation;
                string s = string.Format("  - {0}: {1}", "GeneLabelWithDesc".Translate(psychicManip.def.Named("GENE")).CapitalizeFirst(), psychicManip.skillModifier.ToStringWithSign());
                sb.AppendLine(s);
            }
            SkillRecord = null;
        }


        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            CodeMatcher codeMatcher = new CodeMatcher(instructions);
            //MethodInfo method = AccessTools.Method(typeof(StringBuilder), "AppendLine", new Type[] { typeof(string) });
            MethodInfo injectStringSb = AccessTools.Method(typeof(Patch_SkillUI_GetSkillDescription), "InjectStringSb", new Type[] { typeof(StringBuilder) });
            MethodInfo biotechCondition = AccessTools.PropertyGetter(typeof(ModsConfig), "BiotechActive");

            codeMatcher.MatchStartForward(
                    CodeMatch.Calls(biotechCondition)
                )
                .ThrowIfNotMatchForward("Could not find specific line - Foxian Transpiler GetSkillDescription")
                .Advance()
                .Insert(
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Call, injectStringSb)
                );
            return codeMatcher.Instructions();
        }
    }
}
