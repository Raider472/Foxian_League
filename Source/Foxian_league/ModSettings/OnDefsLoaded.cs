using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    [StaticConstructorOnStartup]
    public class OnDefsLoaded {
        static OnDefsLoaded() {
            // This static constructor will be called when the game loads the defs and will apply the settings to the defs.
            ApplySettingsToDefs();
        }

        public static void ApplySettingsToDefs() {
            if(!Foxian_Settings.IsFoxianBi) {
                Log.Message("Foxian League: IsFoxianBi is false, removing forcedTraits from FL_Lustful");
                InternalDefOf.FL_Lustful.forcedTraits = null;
            }
        }
    }
}
