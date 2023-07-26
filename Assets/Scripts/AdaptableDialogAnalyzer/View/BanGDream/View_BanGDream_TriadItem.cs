using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_TriadItem : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup canvasGroup;
        public UIFollower uIFollower;
        public Image imgCharA;
        public Image imgCharB;
        public List<Image> imgDecoA;
        public List<Image> imgDecoB;
        public IndividualColorElement iceBGColor;
        public IndividualColorElement iceTextColorA;
        public IndividualColorElement iceTextColorB;
        public Text txtTotal;
        public Text txtAToB;
        public Text txtBToA;
        [Header("Settings")]
        public IndexedSpriteList charSDList;
        public IndexedSpriteList charDecoList;
        public IndexedColorList bgColorList;
        public IndexedColorList textColorList;
        public float fadeDuration = 0.5f;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;
        public IndexedHDRColorList hdrColorList;

        View_BanGDream_ItemEffect itemEffect;
        protected View_BanGDream_ItemEffect ItemEffect => itemEffect;

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

        protected void SetGraphics(int characterAId, int characterBId)
        {
            imgCharA.sprite = charSDList[characterAId];
            imgCharB.sprite = charSDList[characterBId];

            foreach (var image in imgDecoA)
            {
                image.sprite = charDecoList[characterAId];
            }
            foreach (var image in imgDecoB)
            {
                image.sprite = charDecoList[characterBId];
            }

            iceBGColor.SetIndividualColor(bgColorList[characterAId]);
            iceTextColorA.SetIndividualColor(textColorList[characterAId]);
            iceTextColorB.SetIndividualColor(textColorList[characterBId]);

            canvasGroup.alpha = 0;
        }
    }
}