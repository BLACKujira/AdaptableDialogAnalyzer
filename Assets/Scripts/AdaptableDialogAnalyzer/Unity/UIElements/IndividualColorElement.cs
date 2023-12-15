using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 自定义色彩的元素，不使用主题色
    /// </summary>
    public class IndividualColorElement : MonoBehaviour
    {
        [Header("Components")]
        public List<Graphic> colorGraphics;
        public List<Graphic> textColorGraphics;
        [Header("Settings")]
        public Color colorWhite = Color.white;
        public Color colorBlack = new Color32(68, 68, 102, 255);
        public float fadeTime = 0.25f;
        [Tooltip("开启后，所有textColorGraphics的shadow设置为individualColor")]
        public bool colorShadow = false;
        public UIElementTheme theme = UIElementTheme.Auto;

        Color individualColor;
        public Color IndividualColor => individualColor;

        Color textColor;
        public Color TextColor => textColor;

        private void Awake()
        {
            individualColor = colorGraphics.Count == 0 ? Color.clear : colorGraphics[0].color;
            textColor = textColorGraphics.Count == 0 ? Color.clear : textColorGraphics[0].color;
        }

        public void SetIndividualColor(Color individualColor, bool reverse = false, bool fade = false)
        {
            if (theme == UIElementTheme.None && reverse)
            {
                Debug.LogWarning("IndividualColorElement: 反向为 true，但主题为 None");
                return;
            }

            this.individualColor = individualColor;

            //计算最高对比度的文字颜色
            Color textColor = Color.white;
            switch (theme)
            {
                case UIElementTheme.None:
                    break;
                case UIElementTheme.Auto:
                    textColor = APCA.GetBlackOrWhite(individualColor, colorWhite, colorBlack);
                    break;
                case UIElementTheme.Light:
                    textColor = colorBlack;
                    break;
                case UIElementTheme.Dark:
                    textColor = colorWhite;
                    break;
            }
            this.textColor = textColor;

            //如果设置了反转则交换两种颜色
            Color colorGraphicsColor = reverse ? textColor : individualColor;
            Color textColorGraphicsColor = reverse ? individualColor : textColor;

            foreach (Graphic graphic in colorGraphics)
            {
                if (graphic != null)
                {
                    if (fade) graphic.DOColor(colorGraphicsColor, fadeTime);
                    else graphic.color = colorGraphicsColor;
                }
            }

            if (theme != UIElementTheme.None)
            {
                foreach (Graphic graphic in textColorGraphics)
                {
                    if (graphic != null)
                    {
                        if (fade) graphic.DOColor(textColorGraphicsColor, fadeTime);
                        else graphic.color = textColorGraphicsColor;
                        if (colorShadow)
                        {
                            Shadow[] shadows = graphic.GetComponents<Shadow>();
                            foreach (var shadow in shadows)
                            {
                                shadow.effectColor = individualColor;
                            }
                        }
                    }
                }
            }
        }
    }
}