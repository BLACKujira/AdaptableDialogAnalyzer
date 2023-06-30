using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_CharacterMentions_ItemTotal : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharIcon;
        public Text txtCharName;
        public Text txtCount;
        [Header("Settings")]
        public IndexedSpriteList charIconList;

        public void SetData(int characterID, int totalMention, int serifCount)
        {
            imgCharIcon.sprite = charIconList[characterID];
            txtCharName.text = GlobalConfig.CharacterDefinition[characterID].name;
            txtCount.text = $"共提及其他角色{totalMention}次，在{serifCount}句台词中";
        }
    }
}