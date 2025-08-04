using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    [StaticConstructorOnStartup]
    public class Comand_ToggleAbility : Command_Toggle {
        public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG");

        public new static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBGShrunk");

        public override Texture2D BGTexture => BGTex;

        public override Texture2D BGTextureShrunk => BGTexShrunk;
    }
}
