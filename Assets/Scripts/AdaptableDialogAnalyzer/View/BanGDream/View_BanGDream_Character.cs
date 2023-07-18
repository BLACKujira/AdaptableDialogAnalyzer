using AdaptableDialogAnalyzer.MaterialController;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Character : MonoBehaviour
    {
        [Header("Components")]
        public View_BanGDream_CharacterMentions characterMentions;
        public View_BanGDream_MostAnalysis mostAnalysis;
        public View_BanGDream_TransitionIn transitionIn;
        public View_BanGDream_TransitionOut transitionOut;
        public MaterialController_HDRColorTexture particleMaterial;
        [Header("Time")]
        public float delayBeforeFadeIn = 1f;
        public float delayAfterFadeIn = 0.5f;
        public float page1Duration = 15;
        public float delayAfterPage1 = 15;
        public float page2Duration = 15;
        [Header("Settings")]
        public IndexedHDRColorList characterColorList;
        public int speakerId = 1;

        private void Start()
        {
            particleMaterial.HDRColor = characterColorList[speakerId];

            characterMentions.speakerId = speakerId;
            mostAnalysis.speakerId = speakerId;

            characterMentions.Initialize();
            mostAnalysis.Initialize();
            transitionIn.Initialize();
            transitionOut.Initialize();
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Space)) 
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
            characterMentions.gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBeforeFadeIn);
            transitionOut.StartTransition();

            yield return new WaitForSeconds(delayAfterFadeIn);
            yield return 1;
            characterMentions.FadeIn();
            
            yield return new WaitForSeconds(page1Duration);
            transitionOut.gameObject.SetActive(false);
            characterMentions.FadeOut();
            
            yield return new WaitForSeconds(delayAfterPage1);
            characterMentions.gameObject.SetActive(false);
            mostAnalysis.gameObject.SetActive(true);
            mostAnalysis.FadeIn();
            
            yield return new WaitForSeconds(page2Duration);
            transitionIn.gameObject.SetActive(true);
            transitionIn.StartTransition();
        }
    }
}