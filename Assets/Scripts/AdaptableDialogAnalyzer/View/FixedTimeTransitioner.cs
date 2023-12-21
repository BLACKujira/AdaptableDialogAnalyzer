using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View
{
    public class FixedTimeTransitioner : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("�ɿ�")] public MonoBehaviour transitionIn;
        [Tooltip("�ɿ�")] public MonoBehaviour transitionOut;
        public List<GameObject> activeOnTransitionInMiddle;
        [Header("Settings")]
        public float holdTime = 10f;
        public bool manualPlay = false;

        bool isPlaying = false;

        private void Start()
        {
            if(!manualPlay)
            {
                Play();
            }
        }

        private void Update()
        {
            if(manualPlay && Input.GetKeyDown(KeyCode.Space))
            {
                Play();
            }
        }

        public void Play()
        {
            if(isPlaying)
            {
                Debug.LogWarning("Transitioner is already playing");
                return;
            }
            StartCoroutine(CoProcess());
        }

        void ActiveObjects()
        {
            foreach (var obj in activeOnTransitionInMiddle)
            {
                obj.SetActive(true);
            }
        }

        IEnumerator CoProcess()
        {
            if(transitionIn != null)
            {
                // ���������жϵ��ò�ͬ��ת������
                if(transitionIn is IFadeIn fadeIn)
                {
                    ActiveObjects();
                    fadeIn.FadeIn();
                }
                else if(transitionIn is ITransition transition)
                {
                    transition.OnTransitionMiddle += ActiveObjects;
                    transition.StartTransition();
                }
                else
                {
                    Debug.LogError("TransitionIn is not IFadeIn or ITransition");
                }
            }

            // �ȴ�holdTime���ִ��ת��
            yield return new WaitForSeconds(holdTime);

            if(transitionOut != null)
            {
                // ���������жϵ��ò�ͬ��ת������
                if (transitionOut is IFadeOut fadeOut)
                {
                    fadeOut.FadeOut();
                }
                else if(transitionOut is ITransition transition)
                {
                    transition.StartTransition();
                }
                else
                {
                    Debug.LogError("TransitionOut is not IFadeOut or ITransition");
                }
            }
        }
    }
}