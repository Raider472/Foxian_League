using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    //Yes, this comp only exists to show the severity in the hediff label as a percentage
    public class Hediff_ShowSeverity : HediffWithComps {
        public override string LabelInBrackets {
            get {
                string labelInBrackets = base.LabelInBrackets;
                string text = Severity.ToStringPercent("F0");
                return labelInBrackets + " - " + text;
            }
        }
    }
}
