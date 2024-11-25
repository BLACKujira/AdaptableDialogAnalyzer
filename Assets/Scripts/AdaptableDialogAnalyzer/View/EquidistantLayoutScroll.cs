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
        public bool stopAtScrollEnd = false;

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

        public event Action<GameObject> onItemEnter; // 当物体进入可视区域时触发的事件
        public event Action<GameObject> onItemExit; // 当物体离开可视区域时触发的事件
        public List<ScrollEvent> scrollItemEvents = new List<ScrollEvent>(); // 自定义滚动事件列表
        public event Action onScrollEnd; // 当滚动到终点时触发的事件

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

        private void Start()
        {
            // 默认绑定滚动结束事件
            onScrollEnd += DefaultOnScrollEnd;
        }

        private void Update()
        {
            // 缓存上一帧所有物体的 X 轴位置
            Dictionary<GameObject, float> lastItemPositions = new Dictionary<GameObject, float>();
            Dictionary<GameObject, float> currItemPositions = new Dictionary<GameObject, float>();

            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                lastItemPositions.Add(item, itemRectTransform.anchoredPosition.x + scrollRectTransform.anchoredPosition.x);
            }

            float lastScrollPosition = -scrollRectTransform.anchoredPosition.x;

            // 控制滚动
            if (enableScroll)
                scrollRectTransform.anchoredPosition = new Vector2(scrollRectTransform.anchoredPosition.x - scrollSpeed * Time.deltaTime, 0f);

            // 缓存当前帧所有物体的 X 轴位置
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                currItemPositions.Add(item, itemRectTransform.anchoredPosition.x + scrollRectTransform.anchoredPosition.x);
            }

            float currScrollPosition = -scrollRectTransform.anchoredPosition.x;

            // 检查物体进入/退出视图的事件
            CheckItemEvents(lastItemPositions, currItemPositions);

            // 检查是否滚动到终点
            CheckScrollEnd(lastScrollPosition, currScrollPosition);
        }


        /// <summary>
        /// 检查物体是否触发进入、退出或其他自定义事件
        /// </summary>
        void CheckItemEvents(Dictionary<GameObject, float> lastItemPositions, Dictionary<GameObject, float> currItemPositions)
        {
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                float lastItemPosition = lastItemPositions[item];
                float currItemPosition = currItemPositions[item];

                // 检查是否进入视图
                if (lastItemPosition > RectTransform.rect.width && currItemPosition <= RectTransform.rect.width)
                {
                    onItemEnter?.Invoke(item);
                }

                // 检查是否退出视图
                float exitPositionX = -itemRectTransform.sizeDelta.x;
                if (lastItemPosition > exitPositionX && currItemPosition <= exitPositionX)
                {
                    onItemExit?.Invoke(item);
                }

                // 检查其他滚动事件
                foreach (var scrollEvent in scrollItemEvents)
                {
                    if (lastItemPosition > scrollEvent.scrollValue && currItemPosition <= scrollEvent.scrollValue)
                    {
                        scrollEvent?.CallEvent(item);
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否滚动到末尾并触发事件
        /// </summary>
        void CheckScrollEnd(float lastScrollPosition, float currScrollPosition)
        {
            float endScrollValue = GetScrollLength() - RectTransform.rect.width;

            if (lastScrollPosition < endScrollValue && currScrollPosition >= endScrollValue)
            {
                onScrollEnd?.Invoke();
            }
        }

        /// <summary>
        /// 获取滚动内容的总长度
        /// </summary>
        float GetScrollLength()
        {
            float endScrollValue = 0;
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                float itemEndPosition = itemRectTransform.anchoredPosition.x + itemRectTransform.sizeDelta.x;
                if (itemEndPosition > endScrollValue)
                {
                    endScrollValue = itemEndPosition;
                }
            }
            return endScrollValue;
        }

        /// <summary>
        /// 默认滚动结束时的行为
        /// </summary>
        void DefaultOnScrollEnd()
        {
            if (stopAtScrollEnd)
            {
                enableScroll = false;
                scrollRectTransform.anchoredPosition = new Vector2(-GetScrollLength() + RectTransform.rect.width, 0f);
            }
        }

        /// <summary>
        /// 对所有进入范围内的元素进行操作
        /// </summary>
        public void ForEachAlreadyEnterRect(Action<GameObject> action, bool excludeExitItems = true)
        {
            ForEachAlreadyEnter(RectTransform.rect.width, action, excludeExitItems);
        }

        /// <summary>
        ///  对所有进入范围内的元素进行操作
        /// </summary>
        public void ForEachAlreadyEnter(float scrollValue, Action<GameObject> action, bool excludeExitItems = true)
        {
            List<GameObject> selectedItems = new List<GameObject>();
            foreach (var item in equidistantLayoutGenerator.Items)
            {
                RectTransform itemRectTransform = GetRectTransform(item);
                float currItemPosition = itemRectTransform.anchoredPosition.x + scrollRectTransform.anchoredPosition.x;
                if (!excludeExitItems || currItemPosition >= 0)
                {
                    if (currItemPosition <= scrollValue)
                    {
                        selectedItems.Add(item);
                    }
                }
            }

            foreach (var item in selectedItems)
            {
                action(item);
            }
        }
    }
}
