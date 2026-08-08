using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class CompProperties_RestorePsyfocusOnKill : CompProperties {
        public FloatRange psyfocusRestoreRange;
        public bool restoreOnNonFleshKill = false;

        public CompProperties_RestorePsyfocusOnKill() {
            compClass = typeof(Comp_RestorePsyfocusOnKill);
        }
    }
}
