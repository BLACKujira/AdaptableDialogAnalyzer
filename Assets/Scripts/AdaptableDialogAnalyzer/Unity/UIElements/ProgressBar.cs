using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// ½ø¶ÈÌõ
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        [Header("Components")]
        public Image imageFill;
        public Text percentText;
        [Header("Settings")]
        public string numberFormat = "0.00";

        public float Priority
        {
            get => imageFill.fillAmount;
            set
            {
                imageFill.fillAmount = value;
                if (percentText)
                    percentText.text = (value * 100f).ToString(numberFormat) + '%';
            }
        }
    }
}