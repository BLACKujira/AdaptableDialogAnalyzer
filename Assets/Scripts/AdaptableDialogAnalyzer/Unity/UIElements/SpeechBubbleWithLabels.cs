using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 带按钮的对话气泡
    /// </summary>
    public class SpeechBubbleWithLabels : SpeechBubbleButton
    {
        public CharacterNamePanelGenerator panelGenerator;
        public List<GameObject> hideObjectsWhileNoLabel;

        public void SetCharacterLabels(int[] characterIds = null)
        {
            if(characterIds == null || characterIds.Length == 0) 
            {
                SetLabelDisplay(false);
            }
            else
            {
                SetLabelDisplay(true);
                panelGenerator.SetData(characterIds);
            }
        }

        private void SetLabelDisplay(bool enabled)
        {
            foreach (var gobj in hideObjectsWhileNoLabel)
            {
                if(gobj.activeSelf != enabled) gobj.SetActive(enabled);
            }
        }
    }
}
