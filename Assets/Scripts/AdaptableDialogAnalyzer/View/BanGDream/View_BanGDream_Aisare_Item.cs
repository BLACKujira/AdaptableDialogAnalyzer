using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Aisare_Item : View_BanGDream_MapItem
    {
        public void SetData(int mentionedPersonId, int countSameUnit,int countOtherUnit)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            imgSpeaker.sprite = charIconList[mentionedPersonId];
            iceBGColor.SetIndividualColor(bgColorList[mentionedPersonId]);
            iceTextColor.SetIndividualColor(textColorList[mentionedPersonId]);

            Color shadowColor = characterDefinition[mentionedPersonId].color;
            shadowColor.a = areaShadowAlpha;
            imgAreaShadow.color = shadowColor;

            txtCount.text = $"組合内: {countSameUnit}  組合外: {countOtherUnit}";

            if(mentionedPersonId == 15)
            {
                txtPercent.text = $"被提到: {countSameUnit + countOtherUnit}次\n(不计米歇尔: 2396)";
            }
            else
            {
                txtPercent.text = $"被提到: {countSameUnit + countOtherUnit}次";
            }

            ItemEffect.materialController.HDRColor = hdrColorList[mentionedPersonId];
            canvasGroup.alpha = 0;
        }
    }
}