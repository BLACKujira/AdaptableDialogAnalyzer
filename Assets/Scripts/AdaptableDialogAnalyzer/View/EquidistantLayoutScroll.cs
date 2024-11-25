using AdaptableDialogAnalyzer.UIElements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View
{
    public class EquidistantLayoutScroll : MonoBehaviour
    {
        [Header("Components")]
        public EquidistantLayoutGenerator equidistantLayoutGenerator;
        public RectTransform scrollRectTransform;
        [Header("Settings")]
        public bool enableScroll = true;
        public float scrollSpeed = 100f;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        public event Action<GameObject> onItemEnter;
        public event Action<GameObject> onItemExit;
        public List<ScrollEvent> scrollEvents = new List<ScrollEvent>();

        Dictionary<GameObject, RectTransform> cachedRectTransforms = new Dictionary<GameObject, RectTransform>();

        /// <summary>
        /// 获取缓存的RectTransform
        /// </summary>
        RectTransform GetRectTransform(GameObject obj)
        {
            if (!cachedRectTransforms.ContainsKey(obj))
            {
                cachedRectTransforms.Add(obj, obj.GetComponent<RectTransform>());
            }
            return cachedRectTransforms[obj];
        }

        private void Update()
        {
            Dictionary<GameObject, float> lastItemPositions = new Dictionary<GameObject, float>();
            Dictionary<GameObject, float> currItemPositions = new Dictionary<GameObject, float>();

            // 缓存上一帧的位置
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                lastItemPositions.Add(item, itemRectTransform.anchoredPosition.x + scrollRectTransform.anchoredPosition.x);
            }

            // 移动卷轴
            scrollRectTransform.anchoredPosition = new Vector2(scrollRectTransform.anchoredPosition.x - scrollSpeed * Time.deltaTime, 0f);

            // 缓存这一帧的位置
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                currItemPositions.Add(item, itemRectTransform.anchoredPosition.x + scrollRectTransform.anchoredPosition.x);
            }

            // 检查是否触发事件
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                float lastItemPosition = lastItemPositions[item];
                float currItemPosition = currItemPositions[item];

                // 检查是否进入
                if (lastItemPosition > RectTransform.rect.width && currItemPosition <= RectTransform.rect.width)
                {
                    onItemEnter?.Invoke(item);
                }

                // 检查是否推出
                float exitPositionX = -itemRectTransform.sizeDelta.x;
                if (lastItemPosition > exitPositionX && currItemPosition <= exitPositionX)
                {
                    onItemExit?.Invoke(item);
                }

                // 检查其余事件
                foreach (var scrollEvent in scrollEvents)
                {
                    if (lastItemPosition > scrollEvent.scrollValue && currItemPosition <= scrollEvent.scrollValue)
                    {
                        scrollEvent?.CallEvent(item);
                    }
                }
            }
        }
    }
}
