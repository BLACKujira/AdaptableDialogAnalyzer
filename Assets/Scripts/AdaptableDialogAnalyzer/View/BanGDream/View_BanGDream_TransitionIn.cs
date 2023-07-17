using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_TransitionIn : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_TransitionIn_Line> lines;
        public Transform tfUIEffect;
        [Header("Settings")]
        public float moveEndX = -300;
        public float moveInterval = 0.2f;
        public float moveDuration = 1;
        public float fadeDuration = 0.8f;
        public float fadeDelay = 0.2f;
        [Header("Prefab")]
        public View_BanGDream_ItemEffect lineEffectPrefab;

        public void Initialize()
        {
            foreach (var line in lines) 
            {
                View_BanGDream_ItemEffect view_BanGDream_ItemEffect = Instantiate(lineEffectPrefab, tfUIEffect);
                line.uIFollower.targetTransform = view_BanGDream_ItemEffect.transform;
            }
        }

        public void StartTransition()
        {
            StartCoroutine(CoStartTransition());
        }

        IEnumerator CoStartTransition()
        {
            foreach (var line in lines) 
            {
                line.RectTransform.DOAnchorPosX(moveEndX, moveDuration);
                line.imgWhiteMask.DOFade(1, fadeDuration).SetDelay(fadeDelay);
                yield return new WaitForSeconds(moveInterval); 
            }
        }
    }
}