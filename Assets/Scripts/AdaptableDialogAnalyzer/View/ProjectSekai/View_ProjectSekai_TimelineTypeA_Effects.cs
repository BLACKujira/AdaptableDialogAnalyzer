using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA_Effects : MonoBehaviour
    {
        [Header("Components")]
        public List<ParticleSystem> timelineParticles = new List<ParticleSystem>();
        public List<ColorTransition> transitions = new List<ColorTransition>();
        public List<GameObject> yearBackgrounds = new List<GameObject>();
        [Header("Settings")]
        public float transitionDelay = 0.5f;

        private void Awake()
        {
            for (int i = 1; i < transitions.Count; i++)
            {
                int year = i;
                transitions[i].OnTransitionMiddle += () =>
                {
                    yearBackgrounds[year - 1].SetActive(false);
                    yearBackgrounds[year].SetActive(true);
                };
            }  
        }

        public void PlayAnniversaryEffect(int anniversary)
        {
            StartCoroutine(CoPlayAnniversaryEffect(anniversary));
        }

        IEnumerator CoPlayAnniversaryEffect(int anniversary)
        {
            timelineParticles[anniversary].Play();
            yield return new WaitForSeconds(transitionDelay);
            transitions[anniversary].StartTransition();
        }
    }
}
