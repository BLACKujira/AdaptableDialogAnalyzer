using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View
{
    public abstract class ColorTransition : MonoBehaviour, ITransition
    {
        public List<GameObject> activeObjects;
        [Header("Settings")]
        public float transitionInDuration = 0.5f;
        public float transitionMiddleHoldTime = 0f;
        public float transitionOutDuration = 0.5f;
        public bool destroyOnEnd = false;

        public event Action OnTransitionMiddle;
        public event Action OnTransitionEnd;

        protected void Awake()
        {
            foreach (var gameObj in activeObjects)
            {
                gameObj.SetActive(false);
            }
        }

        protected abstract void FadeAlpha(float alpha, float duration);

        public void StartTransition()
        {
            StartCoroutine(CoTransition());
        }

        IEnumerator CoTransition()
        {
            foreach (var gameObj in activeObjects)
            {
                gameObj.SetActive(true);
            }

            FadeAlpha(1, transitionInDuration);
            yield return new WaitForSeconds(transitionInDuration);
            OnTransitionMiddle?.Invoke();

            yield return new WaitForSeconds(transitionMiddleHoldTime);

            FadeAlpha(0, transitionInDuration);
            yield return new WaitForSeconds(transitionOutDuration);
            OnTransitionEnd?.Invoke();

            if (destroyOnEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}
