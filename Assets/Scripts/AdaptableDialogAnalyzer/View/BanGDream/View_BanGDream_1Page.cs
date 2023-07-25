using System.Collections;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_1Page : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("必须实现IInitializable，可以实现IFadeIn")] public MonoBehaviour page;
        public View_BanGDream_TransitionIn transitionIn;
        public View_BanGDream_TransitionOut transitionOut;
        [Header("Time")]
        public float delayBeforeFadeIn = 1f;
        public float delayAfterFadeIn = 0.5f;
        public float page1Duration = 13;

        protected virtual void Start()
        {
            IInitializable initializable = page as IInitializable;

            if (initializable == null)
            {
                Debug.Log("page必须实现IInitializable接口");
            }
            else
            {
                initializable.Initialize();
            }

            transitionIn.Initialize();
            transitionOut.Initialize();
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
            transitionOut.gameObject.SetActive(true);
            page.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBeforeFadeIn);
            transitionOut.StartTransition();

            yield return new WaitForSeconds(delayAfterFadeIn);
            yield return 1;
            IFadeIn fade = page as IFadeIn;
            fade?.FadeIn();

            yield return new WaitForSeconds(page1Duration);
            transitionIn.gameObject.SetActive(true);
            transitionIn.StartTransition();
        }
    }
}