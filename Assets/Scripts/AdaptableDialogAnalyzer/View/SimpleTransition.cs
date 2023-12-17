using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View
{

    /// <summary>
    /// 简易转场，在启动时将目标物体设为活动状态
    /// </summary>
    public class SimpleTransition : MonoBehaviour, ITransition
    {
        [Header("Components")]
        public List<GameObject> targetObjects;
        [Header("Settings")]
        public float transitionMiddleDelay = 0.5f;
        [Tooltip("在transitionMiddle后的延迟")]
        public float transitionEndDelay = 0.5f;
        public bool destroyOnEnd = false;

        public event Action OnTransitionMiddle;
        public event Action OnTransitionEnd;

        private void Awake()
        {
            foreach (var gameObj in targetObjects)
            {
                gameObj.SetActive(false);
            }
        }

        public void StartTransition()
        {
            StartCoroutine(CoTransition());
        }

        IEnumerator CoTransition()
        {
            foreach (var gameObj in targetObjects)
            {
                gameObj.SetActive(true);
            }
            yield return new WaitForSeconds(transitionMiddleDelay);
            OnTransitionMiddle?.Invoke();
            yield return new WaitForSeconds(transitionEndDelay);
            OnTransitionEnd?.Invoke();

            if (destroyOnEnd)
            {
                Destroy(gameObject);
            }
        }
    }
}
