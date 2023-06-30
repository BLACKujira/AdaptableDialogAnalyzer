using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_MostMentionedCharacters_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBgColor;
        public Image imgCharL;
        public Image imgCharR;
        public Text txtTitle;
        public Text txtDescription;
        [Header("Settings")]
        public IndexedSpriteList charIconList;

        public void SetData(int speakerId, int mentionedPersonId, string titleText, string descriptionText)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            imgBgColor.color = characterDefinition[mentionedPersonId].color;
            imgCharL.sprite = charIconList[speakerId];
            imgCharR.sprite = charIconList[mentionedPersonId];
            txtTitle.text = titleText;
            txtDescription.text = descriptionText;
        }
    }
}