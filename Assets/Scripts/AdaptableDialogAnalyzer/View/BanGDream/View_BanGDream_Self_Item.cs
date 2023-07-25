using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Self_Item : View_BanGDream_MapItem
    {
        public void SetData(int characterId, int mentionCount, int allMentionCount)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            imgSpeaker.sprite = charIconList[characterId];
            imgMentionedPerson.sprite = charIconList[characterId];
            iceBGColor.SetIndividualColor(bgColorList[characterId]);
            iceTextColor.SetIndividualColor(textColorList[characterId]);

            Color shadowColor = characterDefinition[characterId].color;
            shadowColor.a = areaShadowAlpha;
            imgAreaShadow.color = shadowColor;

            float percent = (float)mentionCount / allMentionCount;
            txtPercent.text = $"{percent * 100:00.00}%";
            txtCount.text = $"{mentionCount} / {allMentionCount}";

            ItemEffect.materialController.HDRColor = hdrColorList[characterId];
            canvasGroup.alpha = 0;
        }
    }
}