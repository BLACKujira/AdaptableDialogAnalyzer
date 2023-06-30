using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_RankingOshi_Item : MonoBehaviour
    {
        [Header("Components")]
        public Image imgCharAIcon;
        public Image imgCharBIcon;
        public Text txtName;
        public Text txtPercent;
        public Text txtCount;
        [Header("Settings")]
        public IndexedSpriteList charIconList;

        public void SetData(int characterAId, int characterBId, int mentionCount, int mentionTotal)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            imgCharAIcon.sprite = charIconList[characterAId];
            imgCharBIcon.sprite = charIconList[characterBId];

            int padWidth = 3;
            char padChar = '　';
            string namaeA = characterDefinition[characterAId].Namae.PadRight(padWidth, padChar);
            string namaeB = characterDefinition[characterBId].Namae.PadRight(padWidth, padChar);
            string colorA = ColorUtility.ToHtmlStringRGB(characterDefinition[characterAId].color);
            string colorB = ColorUtility.ToHtmlStringRGB(characterDefinition[characterBId].color);

            txtName.text = $"<color=#{colorA}>{namaeA}</color> → <color=#{colorB}>{namaeB}</color>";
            txtPercent.text = $"{(float)mentionCount / mentionTotal:00.00}%";
            txtCount.text = $"({mentionCount}/{mentionTotal})";
        }
    }
}