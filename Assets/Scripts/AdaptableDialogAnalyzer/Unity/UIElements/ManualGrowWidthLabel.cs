using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// ��ȸ������ֳ������ӵı�ǩ��
    /// </summary>
    public class ManualGrowWidthLabel : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform rectTransform;
        public Text text;
        [Header("Settings")]
        public float startWidth = 20;
        public float widthDelta = 10;

        /// <summary>
        /// ���֡�
        /// </summary>
        public string Text
        {
            get => text.text;
            set
            {
                text.text = value;
                rectTransform.sizeDelta = new Vector2(startWidth + widthDelta * value.Length, rectTransform.sizeDelta.y);
            }
        }
    }
}