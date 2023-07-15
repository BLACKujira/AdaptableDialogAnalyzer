using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Spine;
using Spine;
using Spine.Unity;
using UnityEngine;
using static AdaptableDialogAnalyzer.Games.BanGDream.GameDefine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_SpineModel : MonoBehaviour
    {
        [Header("Transform")]
        public Vector3 modelPosition;
        public Vector3 modelScale;
        [Header("Prefab")]
        public SkeletonAnimation s000_templete;
        public SkeletonAnimation s000_templete_morfonica;
        public SkeletonAnimation s000_templete_RAS;
        [Header("Settings")]
        public IndexedSpineAtlasList spineAtlasList;
        public int sortingOrder = 80;

        SkeletonAnimation skeletonAnimation;
        public SkeletonAnimation SkeletonAnimation => skeletonAnimation;

        public void SetModel(int characterId)
        {
            SkeletonAnimation skeletonAnimation = null;
            BandIdName bandIdName = BanGDreamHelper.GetCharacterBand(characterId);
            if(bandIdName == BandIdName.None)
            {
                Debug.Log($"不支持的角色{characterId}");
            }

            switch (bandIdName)
            {
                case BandIdName.Morfonica:
                    skeletonAnimation = s000_templete_morfonica;
                    break;
                case BandIdName.RaiseASuilen:
                    skeletonAnimation = s000_templete_RAS;
                    break;
                default:
                    skeletonAnimation = s000_templete;
                    break;
            }

            SkeletonAnimation newSkeletonAnimation = Instantiate(skeletonAnimation,transform);
            this.skeletonAnimation = newSkeletonAnimation;
            newSkeletonAnimation.transform.localScale = modelScale;
            newSkeletonAnimation.transform.localPosition = modelPosition;

            SpineAtlasAsset spineAtlasAsset = spineAtlasList[characterId];

            newSkeletonAnimation.SkeletonDataAsset.atlasAssets[0] = spineAtlasAsset;
            foreach (var atlasAsset in newSkeletonAnimation.skeletonDataAsset.atlasAssets)
            {
                if (atlasAsset != null)
                    atlasAsset.Clear();
                newSkeletonAnimation.skeletonDataAsset.Clear();
            }
            newSkeletonAnimation.Initialize(true);
        }

        public TrackEntry PlayAnimation(string animationName)
        {
            TrackEntry trackEntry = SkeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
            trackEntry.TrackTime = 0;
            return trackEntry;
        }
    }
}