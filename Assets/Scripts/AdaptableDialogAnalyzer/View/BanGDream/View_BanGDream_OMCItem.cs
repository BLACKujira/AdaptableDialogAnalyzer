using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Live2D2;
using AdaptableDialogAnalyzer.MaterialController;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCItem : MonoBehaviour
    {
        [Header("Components")]
        public RawImage rimgLive2D;
        public RawImage rimgLive2DOutline;
        public Image ImgNameLabel;
        public Image ImgNameLabelCover;
        public Text txtCount;
        public Text txtRank;
        public Text txtPercent;
        public Text txtRankPercent;
        public CanvasGroup cgNameLabelCover;
        public CanvasGroup cgCountAreaCover;
        public IndividualColorElement iceBGColor;
        public IndividualColorElement iceThemeColor;
        [Header("Settings")]
        public IndexedModelInfoList indexedModelInfoList;
        public SpriteList nameLabelSpriteList;
        public SpriteList nameLabelCoverSpriteList;
        public IndexedLive2D2AnimationSequenceList animationSequenceList;
        public IndexedColorList BGColorList;
        public float fadeDuration = 0.5f;
        public float animationDelay = 1f;
        public float deltaRankTextBiggerThan19 = 1f;
        public Color startThemeColor = Color.black;

        SimpleL2D2MutiModelManager mutiModelManager;
        SimpleMentionCountResultItemWithRank mentionCountResultItem;
        ModelInstanceInfo modelInstanceInfo;
        List<Live2D2Animation> animationSequence = new List<Live2D2Animation>();

        public void Initlize(SimpleMentionCountResultItemWithRank mentionCountResultItem, SimpleL2D2MutiModelManager mutiModelManager)
        {
            // 设置本地变量
            this.mutiModelManager = mutiModelManager;
            this.mentionCountResultItem = mentionCountResultItem;
            this.animationSequence = animationSequenceList[mentionCountResultItem.characterID]?.animationSequence;

            // 设置UI元素
            string nameLabelSpriteName = $"name_top_chr{mentionCountResultItem.characterID:00}";
            ImgNameLabel.sprite = nameLabelSpriteList[nameLabelSpriteName];
            ImgNameLabelCover.sprite = nameLabelCoverSpriteList[nameLabelSpriteName];
            txtCount.text = mentionCountResultItem.count.ToString();
            txtPercent.text = $"{mentionCountResultItem.Percent * 100:00.00}%";

            txtRank.text = mentionCountResultItem.rank.ToString();
            if (mentionCountResultItem.rank > 19)
                txtCount.rectTransform.anchoredPosition = new Vector2(txtCount.rectTransform.anchoredPosition.x + deltaRankTextBiggerThan19, txtCount.rectTransform.anchoredPosition.y); // 当数字大于19时,调整位置
            txtRankPercent.text = mentionCountResultItem.percentRank.ToString();
            if (mentionCountResultItem.percentRank > 19)
                txtRankPercent.rectTransform.anchoredPosition = new Vector2(txtRankPercent.rectTransform.anchoredPosition.x + deltaRankTextBiggerThan19, txtRankPercent.rectTransform.anchoredPosition.y); // 当数字大于19时,调整位置
            
            iceBGColor.SetIndividualColor(BGColorList[mentionCountResultItem.characterID]);
            iceThemeColor.SetIndividualColor(startThemeColor);
            rimgLive2DOutline.color = GlobalConfig.CharacterDefinition[mentionCountResultItem.characterID].color;

            // 设置Live2D
            IndexedModelInfo indexedModelInfo = indexedModelInfoList[mentionCountResultItem.characterID];
            ModelInstanceInfo modelInstanceInfo = mutiModelManager.AddModel(indexedModelInfo.modelInfo);
            this.modelInstanceInfo = modelInstanceInfo;

            // 设置RawImage
            rimgLive2D.texture = modelInstanceInfo.renderTexture;
            rimgLive2DOutline.texture = modelInstanceInfo.renderTexture;

            // 设置初始状态
            SetInitState();
        }

        /// <summary>
        ///  将各组件的样式设置为初始状态，并停止模型和相机
        /// </summary>
        public void SetInitState()
        {
            rimgLive2D.color = Color.black;
            rimgLive2DOutline.color = new Color(rimgLive2DOutline.color.r, rimgLive2DOutline.color.g, rimgLive2DOutline.color.b, 1);
            cgNameLabelCover.alpha = 1;
            cgCountAreaCover.alpha = 1;

            modelInstanceInfo.simpleLive2DModel.gameObject.SetActive(false);
            modelInstanceInfo.live2DCamera.gameObject.SetActive(false);
        }

        /// <summary>
        /// 启用模型和相机
        /// </summary>
        public void ActiveModel()
        {
            modelInstanceInfo.simpleLive2DModel.gameObject.SetActive(true);
            modelInstanceInfo.live2DCamera.gameObject.SetActive(true);

            if (animationSequence != null && animationSequence[0] != null)
            {
                if(animationSequence[0].Live2DExpression != null)
                    modelInstanceInfo.simpleLive2DModel.PlayExpression(animationSequence[0].Live2DExpression);
                if(animationSequence[0].live2DMotion != null)
                    modelInstanceInfo.simpleLive2DModel.PlayMotion(animationSequence[0].Live2DMotion);
            }
        }

        /// <summary>
        /// 禁用模型和相机
        /// </summary>
        public void InactiveModel()
        {
            modelInstanceInfo.simpleLive2DModel.gameObject.SetActive(false);
            modelInstanceInfo.live2DCamera.gameObject.SetActive(false);
        }

        // 淡入UI
        public void FadeIn()
        {
            rimgLive2D.DOColor(Color.white, fadeDuration);
            rimgLive2DOutline.DOFade(0, fadeDuration);
            cgNameLabelCover.DOFade(0, fadeDuration);
            cgCountAreaCover.DOFade(0, fadeDuration);

            StartCoroutine(CoFadeThemeColor());
            StartCoroutine(CoPlayAnimation());
        }

        IEnumerator CoFadeThemeColor()
        {
            for(float i = 0;  i < fadeDuration; i += Time.deltaTime)
            {
                iceThemeColor.SetIndividualColor(Color.Lerp(startThemeColor, GlobalConfig.CharacterDefinition[mentionCountResultItem.characterID].color, i / fadeDuration));
                yield return null;
            }
        }

        IEnumerator CoPlayAnimation()
        {
            yield return new WaitForSeconds(animationDelay);
            modelInstanceInfo.simpleLive2DModel.PlayExpression(animationSequence[1].Live2DExpression);
            modelInstanceInfo.simpleLive2DModel.PlayMotion(animationSequence[1].Live2DMotion);
        }
    }
}