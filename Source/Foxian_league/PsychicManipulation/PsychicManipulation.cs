using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Foxian_league {
    public class PsychicManipulation: DefModExtension {
        public int skillModifier = 2;
        public string skillName;

        public SkillDef skillDef;
        public Aptitude aptitude;

        public PsychicManipulation() {

        }

        public PsychicManipulation(int skillModifier, string skillName) {
            this.skillModifier = skillModifier;
            this.skillName = skillName;

            this.skillDef = DefDatabase<SkillDef>.GetNamed(skillName);
            this.aptitude = new Aptitude(this.skillDef, this.skillModifier);
        }
    }
}
