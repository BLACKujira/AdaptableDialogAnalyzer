using AdaptableDialogAnalyzer.MaterialController;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Band : MonoBehaviour
    {
        [Header("Components")]
        public View_BanGDream_BandFriends bandFriends;
        public View_BanGDream_TransitionIn transitionIn;
        public View_BanGDream_TransitionOut transitionOut;
        public MaterialController_HDRColorTexture particleMaterial;
        [Header("Time")]
        public float delayBeforeFadeIn = 1f;
        public float delayAfterFadeIn = 0.5f;
        public float page1Duration = 13;
        [Header("Settings")]
        public IndexedHDRColorList bandColorList;

        private void Start()
        {
            particleMaterial.HDRColor = bandColorList[(int)bandFriends.band];

            bandFriends.Initialize();
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
            bandFriends.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBeforeFadeIn);
            transitionOut.StartTransition();

            yield return new WaitForSeconds(delayAfterFadeIn);
            yield return 1;
            bandFriends.FadeIn();

            yield return new WaitForSeconds(page1Duration);
            transitionIn.gameObject.SetActive(true);
            transitionIn.StartTransition();
        }
    }
}