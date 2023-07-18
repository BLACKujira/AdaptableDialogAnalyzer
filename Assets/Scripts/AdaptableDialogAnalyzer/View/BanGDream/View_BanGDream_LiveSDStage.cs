using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;
using static AdaptableDialogAnalyzer.Games.BanGDream.GameDefine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_LiveSDStage : MonoBehaviour
    {
        [Header("Components")]
        public Image imgLiveSD;
        public Image imgStage;
        public UIFollower uIFollower;
        [Header("Settings")]
        public IndexedSpriteList stageSpriteList;
        public IndexedSpriteList liveSDSpriteList;
        public IndexedHDRColorList hdrColorList;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;
        [Header("Swing")]
        public float swingAngle = 5;
        public float swingSpeed = 0.5f;

        View_BanGDream_ItemEffect itemEffect;

        public void Initialize(Transform effectTransform)
        {
            itemEffect = Instantiate(fadeInEffect, effectTransform);
            uIFollower.TargetTransform = itemEffect.transform;
        }

        public void SetData(int characterId)
        {
            imgLiveSD.sprite = liveSDSpriteList[characterId];

            BandIdName bandIdName = BanGDreamHelper.GetCharacterBand(characterId);
            imgStage.sprite = stageSpriteList[(int)bandIdName];

            itemEffect.materialController.HDRColor = hdrColorList[(int)bandIdName];
        }

        public void FadeIn()
        {
            itemEffect.particle.Play();
        }

        private void Update()
        {
            float angle = Mathf.Sin(swingSpeed * Time.time);
            angle = Mathf.Sin(Mathf.PI / 2f * angle);
            angle *= swingAngle;
            imgLiveSD.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}