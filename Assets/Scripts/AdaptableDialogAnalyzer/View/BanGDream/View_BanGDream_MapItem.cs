using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MapItem : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup canvasGroup;
        public UIFollower uIFollower;
        public Image imgSpeaker;
        public Image imgMentionedPerson;
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