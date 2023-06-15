using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 生成一列水平/垂直的等距UI
    /// </summary>
    public class EquidistantLayoutGenerator : MonoBehaviour
    {
        public enum Direction { Vertical, Horizontal }

        [Header("Components")]
        public RectTransform scorllContent;
        [Header("Prefab")]
        public GameObject itemPrefab;
        [Header("Settings")]
        public float blank;
        public float distance;
        public Direction direction = Direction.Vertical;
        public bool reverse = false;

        List<GameObject> items = new List<GameObject>();
        public List<GameObject> Items => items;

        public void Generate(int count, Action<GameObject, int> initialize)
        {
            scorllContent.sizeDelta = direction == Direction.Vertical ?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2) :
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);
            for (int i = 0; i < count; i++)
            {
                int id = i;
                GameObject item = Instantiate(itemPrefab, scorllContent);

                float posX = direction == Direction.Vertical ? 0f : reverse ? -distance * i - blank : distance * i + blank;
                float posY = direction == Direction.Vertical ? reverse ? distance * i + blank : -distance * i - blank : 0f;

                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
                initialize(item, id);
                items.Add(item);
            }
        }

        public void ClearItems()
        {
            foreach (var item in items)
            {
                Destroy(item);
            }
            items.Clear();
        }

        public void AddItem(GameObject prefab, Action<GameObject> initialize)
        {
            float count = items.Count + 1;
            scorllContent.sizeDelta = direction == Direction.Vertical ?
                new Vector2(scorllContent.sizeDelta.x, (count - 1) * distance + blank * 2) :
                new Vector2((count - 1) * distance + blank * 2, scorllContent.sizeDelta.y);

            count--;
            GameObject item = Instantiate(prefab, scorllContent);
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();

            float posX = direction == Direction.Vertical ? 0f : reverse ? -distance * count - blank : distance * count + blank;
            float posY = direction == Direction.Vertical ? reverse ? distance * count + blank : -distance * count - blank : 0f;

            itemRectTransform.anchoredPosition = new Vector2(posX, posY);
            if (initialize != null) initialize(item);
            items.Add(item);
        }

        public void AddItem(Action<GameObject> initialize)
        {
            AddItem(itemPrefab, initialize);
        }
    }
}