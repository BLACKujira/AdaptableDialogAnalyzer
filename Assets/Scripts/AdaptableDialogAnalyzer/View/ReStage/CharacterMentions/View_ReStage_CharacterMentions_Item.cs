using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_CharacterMentions_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharIcon;
        public Text txtCharName;
        public Text txtCount;
        public Text txtPercent;
        [Header("Settings")]
        public IndexedSpriteList charIconList;

        public void SetData(int characterID, int count, int total)
        {
            imgCharIcon.sprite = charIconList[characterID];
            txtCharName.text = GlobalConfig.CharacterDefinition[characterID].name.Replace(" ","");
            txtCharName.color = GlobalConfig.CharacterDefinition[characterID].color;
            txtCount.text = count.ToString();
            txtPercent.text = $"{((float)count / total) * 100:00.00}%";
        }
    }
}