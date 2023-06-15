using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AdaptableDialogAnalyzer.UIElements
{

    /// <summary>
    /// 以网格状生成物体
    /// </summary>
    public class GridGenerator : MonoBehaviour
    {
        private List<GameObject> objectList = new List<GameObject>();
        [Header("Components")]
        [SerializeField] private RectTransform targetContent;
        [Header("Settings")]
        [SerializeField] private int numberPerLine;
        [SerializeField] private float distanceX;
        [SerializeField] private float distanceY;
        [SerializeField] private float offsetX;
        [SerializeField] private float offsetY;

        public List<GameObject> ObjectList => objectList;

        /// <summary>
        /// 以网格形式生成多个物体并初始化
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="count"></param>
        /// <param name="initialize"></param>
        public void Generate(GameObject prefab ,int count, Action<GameObject, int> initialize)
        {
            for (int i = 0; i < count; i++)
            {
                int id = i;
                GameObject gobj = Instantiate(prefab, targetContent);
                initialize(gobj, id);
                gobj.GetComponent<RectTransform>().anchoredPosition = GetPosition(id);
                objectList.Add(gobj);
            }
            RecalculateSize();
        }

        /// <summary>
        /// 计算第id个物体所在的位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Vector2 GetPosition(int id)
        {
             return new Vector2(
                distanceX * (id % numberPerLine) + offsetX,
                -distanceY * (id / numberPerLine) - offsetY);
        }

        private void RecalculateSize()
        {
            targetContent.sizeDelta = new Vector2(
                targetContent.sizeDelta.x,
                distanceY * ((objectList.Count / numberPerLine) + ((objectList.Count % numberPerLine) == 0 ? 0 : 1)) + offsetY
                );
        }

        /// <summary>
        /// 清除所有物体
        /// </summary>
        public void Clear()
        {
            foreach (var gobj in objectList)
            {
                Destroy(gobj);
            }
            objectList = new List<GameObject>();
            RecalculateSize();
        }

        /// <summary>
        /// 在网格的最后生成一个物体
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initialize"></param>
        public void AddButton(GameObject prefab, Action<GameObject> initialize)
        {
            GameObject gobj = Instantiate(prefab, targetContent);
            initialize(gobj);
            gobj.GetComponent<RectTransform>().anchoredPosition = GetPosition(objectList.Count);
            objectList.Add(gobj);
            RecalculateSize();
        }

        /// <summary>
        /// 增减ObjectList中的对象后使用，重新排列对象
        /// </summary>
        public void ResetPosition()
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
            }
            RecalculateSize();
        }
    }
}