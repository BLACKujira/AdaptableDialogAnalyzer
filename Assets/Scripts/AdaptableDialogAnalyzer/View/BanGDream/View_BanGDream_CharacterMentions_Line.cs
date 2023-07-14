using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_CharacterMentions_Line : MonoBehaviour
    {
        [Header("Components")]
        public List<ItemBundle> itemBundles;
        [Header("Time")]
        public float moveInterval = 0.066f;
        public float moveDuration = 1;

        [Serializable]
        public class ItemBundle
        {
            public View_BanGDream_CharacterMentions_Item item;
            public float startX;
            [NonSerialized] public float endX;
        }

        private void Start()
        {
            foreach (var itemBundle in itemBundles) 
            {
                itemBundle.endX = itemBundle.item.RectTransform.anchoredPosition.x;
                Vector2 anchoredPosition = itemBundle.item.RectTransform.anchoredPosition;
                anchoredPosition.x = itemBundle.startX;
                itemBundle.item.RectTransform.anchoredPosition = anchoredPosition;
            }
        }

        public void Move()
        {
            StartCoroutine(CoMove());
        }

        IEnumerator CoMove()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(moveInterval);
            foreach (var itemBundle in itemBundles)
            {
                itemBundle.item.RectTransform.DOAnchorPosX(itemBundle.endX,moveDuration).SetEase(Ease.OutBack);
                yield return waitForSeconds;
            }
        }
    }
}