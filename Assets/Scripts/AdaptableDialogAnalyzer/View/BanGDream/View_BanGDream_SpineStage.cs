using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using Spine.Unity;
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
        [Header("Settings")]
        public IndexedSpriteList stageSpriteList;
        public AnimationReferenceAsset animationReference;

        public void SetModel(int characterId)
        {
            spineModel.SetModel(characterId);
            spineModel.PlayAnimation(animationReference.name);
            BandIdName bandIdName = BanGDreamHelper.GetCharacterBand(characterId);
            imgStage.sprite = stageSpriteList[(int)bandIdName];
        }
    }
}