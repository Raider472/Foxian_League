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
    }
}
