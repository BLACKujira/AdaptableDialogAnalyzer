using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_Item_InfoBar : MonoBehaviour
    {
        [Header("Components")]
        public Image imgBG;
        public Image imgSdCharaL;
        public Image imgSdCharaR;
        public Text txtInfo;
        public UIFollower uIFollower;
        [Header("Settings")]
        public IndexedSpriteList sdCharaList;
        public IndexedColorList bgColorList;
        public IndexedColorList textColorList;
        public IndexedHDRColorList hdrColorList;
        public float colorFadeTime = 1f;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;

        View_BanGDream_ItemEffect itemEffect;
        int characterLID; int characterRID;

        public void Initialize(Transform effectTransform)
        {
            itemEffect = Instantiate(fadeInEffect,effectTransform);
            uIFollower.TargetTransform = itemEffect.transform;
        }

        public void SetData(int characterLID, int characterRID, string info)
        {
            this.characterLID = characterLID;
            this.characterRID = characterRID;

            imgSdCharaL.sprite = sdCharaList[characterLID];
            imgSdCharaR.sprite = sdCharaList[characterRID];
            txtInfo.text = info;

            imgBG.color = bgColorList[characterRID];
            txtInfo.color = textColorList[characterRID];
            itemEffect.materialController.HDRColor = hdrColorList[characterRID];
        }

        public void FadeIn()
        {
            itemEffect.particle.Play();
        }
    }
}