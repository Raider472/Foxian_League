using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    public class Foxian_ModSettings: Mod {
        internal static Foxian_Settings Settings;
        public Foxian_ModSettings(ModContentPack content) : base(content) {
            Settings = GetSettings<Foxian_Settings>();
        }

        public override string SettingsCategory() {
            return "FoxianModName".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect) {
            Foxian_Settings.WindowContents(inRect);
        }
    }
}
