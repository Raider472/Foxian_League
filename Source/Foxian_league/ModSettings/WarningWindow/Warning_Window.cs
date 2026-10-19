using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Foxian_league {
    public class Warning_Window : Window {

        private const string LinkToCopy = "https://github.com/Raider472/Foxian_League/tree/version_1.0";
        public Warning_Window() {
            forcePause = true;
            doCloseX = false;
            doCloseButton = false;
            closeOnClickedOutside = false;
            absorbInputAroundWindow = true;
        }
        public override Vector2 InitialSize => new Vector2(600f, 500f);

        public override void DoWindowContents(Rect inRect) {
            Text.Font = GameFont.Medium;
            TextAnchor anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(0, 0, inRect.width, 50), "FoxianModWarningTitle".Translate());
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(new Rect(0, 50, inRect.width, inRect.height - 100), "FoxianModWarningText".Translate());
            Text.Anchor = anchor;

            float buttonY = inRect.height - 80f;
            float buttonHeight = 50f;
            float closetWidth = 400f;
            float copyWidth = 150;
            float spacing = 10f;

            float totalWidth = closetWidth + spacing + copyWidth;
            float startX = (inRect.width - totalWidth) / 2f;
            if(Widgets.ButtonText(new Rect(startX, buttonY, closetWidth, buttonHeight), "FoxianModWarningAccept".Translate())) {
                Foxian_Settings.hasWarningBeenShown = true;
                Foxian_ModSettings.Settings.Write();
                Log.Message("[Foxian League]: Warning window closed, setting hasWarningBeenShown to true.");
                Close();
            }
            if(Widgets.ButtonText(new Rect(startX + closetWidth + spacing, buttonY, copyWidth, buttonHeight), "FoxianCopyLinkToClipboard".Translate())) {
                GUIUtility.systemCopyBuffer = LinkToCopy;

                Messages.Message(
                    "FoxianLinkCopied".Translate(),
                    MessageTypeDefOf.CautionInput,
                    false
                );
            }
        }
    }
}
