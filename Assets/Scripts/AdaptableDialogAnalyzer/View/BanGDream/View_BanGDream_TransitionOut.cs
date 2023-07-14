using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_TransitionOut : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_TransitionOut_Line> lines;
        public Transform tfUIEffect;
        [Header("Settings")]
        public float moveEndX = -300;
        public float moveInterval = 0.2f;
        public float moveDuration = 1;
        [Header("Prefab")]
        public View_BanGDream_ItemEffect lineEffectPrefab;

        private void Start()
        {
            foreach (var line in lines)
            {
                View_BanGDream_ItemEffect view_BanGDream_ItemEffect = Instantiate(lineEffectPrefab, tfUIEffect);
                line.Initialize(view_BanGDream_ItemEffect);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartTransition();
            }
        }

        void StartTransition()
        {
            StartCoroutine(CoStartTransition());
        }

        IEnumerator CoStartTransition()
        {
            foreach (var line in lines)
            {
                line.RectTransform.DOAnchorPosX(moveEndX, moveDuration);
                yield return new WaitForSeconds(moveInterval);
            }
        }
    }
}