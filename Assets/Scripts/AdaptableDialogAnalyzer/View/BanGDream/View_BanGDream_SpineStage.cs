using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;
using static AdaptableDialogAnalyzer.Games.BanGDream.GameDefine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_SpineStage : MonoBehaviour
    {
        [Header("Components")]
        public View_BanGDream_SpineModel spineModel;
        public Image imgStage;
        public UIFollower uIFollower;
        [Header("Settings")]
        public IndexedSpriteList stageSpriteList;
        public AnimationReferenceAsset animationReference;
        public IndexedHDRColorList hdrColorList;
        [Header("Effect")]
        public View_BanGDream_ItemEffect fadeInEffect;

        View_BanGDream_ItemEffect itemEffect;

        public void Initialize(Transform effectTransform)
        {
            itemEffect = Instantiate(fadeInEffect, effectTransform);
            uIFollower.TargetTransform = itemEffect.transform;
        }

        public void SetModel(int characterId, Transform spineTransform)
        {
            spineModel.SetModel(characterId);
            spineModel.SkeletonAnimation.transform.SetParent(spineTransform);
            spineModel.PlayAnimation(animationReference.name);
            BandIdName bandIdName = BanGDreamHelper.GetCharacterBand(characterId);
            imgStage.sprite = stageSpriteList[(int)bandIdName];

            itemEffect.materialController.HDRColor = hdrColorList[(int)bandIdName];
        }

        public void FadeIn()
        {
            itemEffect.particle.Play();
        }
    }
}