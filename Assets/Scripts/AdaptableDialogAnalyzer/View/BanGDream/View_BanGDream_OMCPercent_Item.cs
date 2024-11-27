using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{

    public class View_BanGDream_OMCPercent_Item : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup canvasGroup;
        public UIFollower uIFollower;
        public Text txtRank;
        public Image imgSpeaker;
        public Image imgAreaShadow;
        public IndividualColorElement iceBGColor;
        public IndividualColorElement iceTextColor;
        public Text txtPercent;
        public Text txtCount;
        [Header("Settings")]
        public IndexedSpriteList charIconList;
        public IndexedColorList bgColorList;
        public IndexedColorList textColorList;
        public float areaShadowAlpha = 0.5f;
        public float fadeDuration = 0.5f;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;
        public IndexedHDRColorList hdrColorList;

        View_BanGDream_ItemEffect itemEffect;
        protected View_BanGDream_ItemEffect ItemEffect => itemEffect;

        public void SetData(SimpleMentionCountResultItemWithRank countResultItem)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            txtRank.text = countResultItem.percentRank.ToString();

            imgSpeaker.sprite = charIconList[countResultItem.characterID];
            iceBGColor.SetIndividualColor(bgColorList[countResultItem.characterID]);
            iceTextColor.SetIndividualColor(textColorList[countResultItem.characterID]);

            Color shadowColor = characterDefinition[countResultItem.characterID].color;
            shadowColor.a = areaShadowAlpha;
            imgAreaShadow.color = shadowColor;

            txtPercent.text = $"{countResultItem.Percent * 100:00.00}%";
            txtCount.text = $"{countResultItem.count} / {countResultItem.serifCount}";

            ItemEffect.materialController.HDRColor = hdrColorList[countResultItem.characterID];
            canvasGroup.alpha = 0;
        }

        /// <summary>
        /// 以总数统计模式设置内容 
        /// </summary>
        public void SetData_Count(SimpleMentionCountResultItemWithRank countResultItem)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            txtRank.text = countResultItem.rank.ToString();

            imgSpeaker.sprite = charIconList[countResultItem.characterID];
            iceBGColor.SetIndividualColor(bgColorList[countResultItem.characterID]);
            iceTextColor.SetIndividualColor(textColorList[countResultItem.characterID]);

            Color shadowColor = characterDefinition[countResultItem.characterID].color;
            shadowColor.a = areaShadowAlpha;
            imgAreaShadow.color = shadowColor;

            txtPercent.text = $"{countResultItem.count}";
            txtCount.text = $"{countResultItem.count} / {countResultItem.serifCount}";

            ItemEffect.materialController.HDRColor = hdrColorList[countResultItem.characterID];
            canvasGroup.alpha = 0;
        }

        public void Initialize(Transform effectTransform)
        {
            itemEffect = Instantiate(fadeInEffect, effectTransform);
            uIFollower.TargetTransform = itemEffect.transform;
        }

        public void FadeIn()
        {
            itemEffect.particle.Play();
            canvasGroup.DOFade(1, fadeDuration);
        }
    }
}