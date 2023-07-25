using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Oshi_Item : View_BanGDream_MapItem
    {
        public void SetData(int speakerId, int mentionedPersonId, int mentionCount, int allMentionCount, int mentionSelfCount)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            imgSpeaker.sprite = charIconList[speakerId];
            imgMentionedPerson.sprite = charIconList[mentionedPersonId];
            iceBGColor.SetIndividualColor(bgColorList[speakerId]);
            iceTextColor.SetIndividualColor(textColorList[speakerId]);

            Color shadowColor = characterDefinition[speakerId].color;
            shadowColor.a = areaShadowAlpha;
            imgAreaShadow.color = shadowColor;

            float percent = (float)mentionCount / (allMentionCount - mentionSelfCount);
            txtPercent.text = $"{percent * 100:00.00}%";
            txtCount.text = $"{mentionCount} / ( {allMentionCount} - {mentionSelfCount} )";

            ItemEffect.materialController.HDRColor = hdrColorList[speakerId];
            canvasGroup.alpha = 0;
        }
    }
}