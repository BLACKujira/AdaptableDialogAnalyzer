using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Tip : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform rtWindow;
        public CanvasGroup cgWindow;
        public View_BanGDream_TransitionIn transitionIn;
        [Header("Time")]
        public float delayBeforeFadeIn = 1;
        public float fadeInDuration = 0.8f;
        public float page1Duration = 30;
        [Header("Settings")]
        public float windowStartScale = 0.8f;

        private void Start()
        {
            transitionIn.Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Play();
            }
        }

        public void Play()
        {
            StartCoroutine(CoPlay());
        }

        IEnumerator CoPlay()
        {
            rtWindow.localScale = new Vector3(windowStartScale, windowStartScale, 1);
            cgWindow.alpha = 0;

            yield return new WaitForSeconds(delayBeforeFadeIn);
            rtWindow.DOScale(Vector3.one, fadeInDuration);
            cgWindow.DOFade(1,fadeInDuration);

            yield return new WaitForSeconds(page1Duration);
            transitionIn.gameObject.SetActive(true);
            transitionIn.StartTransition();
        }
    }
}