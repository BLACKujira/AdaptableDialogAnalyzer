using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCMostChapter_Item : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup canvasGroup;
        public UIFollower uIFollower;
        public Text txtRank;
        public Text txtTitle;
        public Image imgAreaShadow;
        public IndividualColorElement iceBGColor;
        public IndividualColorElement iceTextColor;
        public Text txtCount;
        public Text txtDetail;
        [Header("Settings")]
        public IndexedColorList bgColorList;
        public IndexedColorList textColorList;
        public float areaShadowAlpha = 0.5f;
        public float fadeDuration = 0.5f;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;
        public IndexedHDRColorList hdrColorList;

        View_BanGDream_ItemEffect itemEffect;
        protected View_BanGDream_ItemEffect ItemEffect => itemEffect;

        public void SetData(ObjectMentionedCountMutiMatrix countMatrix, int characterId, int rank)
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            txtRank.text = rank.ToString();
            txtTitle.text = $"{countMatrix.chapterInfo.chapterType}: {countMatrix.chapterInfo.chapterTitle}";

            iceBGColor.SetIndividualColor(bgColorList[characterId]);
            iceTextColor.SetIndividualColor(textColorList[characterId]);

            Color shadowColor = characterDefinition[characterId].color;
            shadowColor.a = areaShadowAlpha;
            imgAreaShadow.color = shadowColor;

            txtCount.text = $"{countMatrix[characterId].Count}  省略号";
            txtDetail.text = $"{countMatrix[characterId].Count} 省略号 / {countMatrix[characterId].serifCount} 台词   平均每句台词 {(float) countMatrix[characterId].Count / countMatrix[characterId].serifCount:0.00} 个省略号";

            ItemEffect.materialController.HDRColor = hdrColorList[characterId];
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

        public void FadeOut()
        {
            canvasGroup.DOFade(0, fadeDuration);
        }
    }
}
