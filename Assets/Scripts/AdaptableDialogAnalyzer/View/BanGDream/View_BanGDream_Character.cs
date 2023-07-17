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
        [Header("Time")]
        public float delayBeforeFadeIn = 1f;
        public float delayAfterFadeIn = 0.5f;
        public float page1Duration = 15;
        public float delayAfterPage1 = 15;
        public float page2Duration = 15;

        private void Start()
        {
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
            yield return new WaitForSeconds(delayBeforeFadeIn);
            transitionIn.StartTransition();
            yield return new WaitForSeconds(delayAfterFadeIn);
            characterMentions.FadeIn();
            yield return new WaitForSeconds(page1Duration);
            characterMentions.FadeOut();
            yield return new WaitForSeconds(delayAfterPage1);
            mostAnalysis.FadeIn();
            yield return new WaitForSeconds(page2Duration);
            transitionOut.StartTransition();
        }
    }
}