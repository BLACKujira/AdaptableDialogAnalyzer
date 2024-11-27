using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCPercent_Title : MonoBehaviour
    {
        [Header("Components")]
        public Image imgTitleBG;
        public Text txtTitle;
        public Text txtSubTitle;
        [Header("Settings")]
        public float titleBGFadeDuration = 1f;
        public float titleFadeDelay = 1f;
        public float titleFadeDuration = 1f;
        public float subTitleFadeDelay = 1f;
        public float subTitleFadeDuration = 1f;
        public float titleMoveDelay = 1f;
        public float titleMoveDuration = 2f;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        Vector2 targetPosition;
        Vector2 targetTitleBGSize;

        public void Initialize()
        {
            targetPosition = RectTransform.anchoredPosition;
            RectTransform.anchoredPosition = Vector2.zero;
            targetTitleBGSize = imgTitleBG.rectTransform.sizeDelta;
            imgTitleBG.color = new Color(imgTitleBG.color.r, imgTitleBG.color.g, imgTitleBG.color.b, 0f);
            imgTitleBG.rectTransform.sizeDelta = new Vector2(0, imgTitleBG.rectTransform.sizeDelta.y);
            txtTitle.color = new Color(txtTitle.color.r, txtTitle.color.g, txtTitle.color.b, 0f);
            txtSubTitle.color = new Color(txtSubTitle.color.r, txtSubTitle.color.g, txtSubTitle.color.b, 0f);
        }

        public void FadeIn()
        {
            StartCoroutine(CoFadeIn());
        }

        IEnumerator CoFadeIn()
        {
            imgTitleBG.rectTransform.DOSizeDelta(targetTitleBGSize, titleBGFadeDuration);
            imgTitleBG.DOFade(1, titleBGFadeDuration);

            yield return new WaitForSeconds(titleFadeDelay);
            txtTitle.DOFade(1, titleFadeDuration);

            yield return new WaitForSeconds(subTitleFadeDelay);
            txtSubTitle.DOFade(1, subTitleFadeDuration);

            yield return new WaitForSeconds(titleMoveDelay);
            RectTransform.DOAnchorPos(targetPosition, titleMoveDuration).SetEase(Ease.OutCubic);
        }
    }
}