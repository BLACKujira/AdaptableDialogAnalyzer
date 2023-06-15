using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class GlobalColor : MonoBehaviour
    {
        public Color themeColor = new Color32(51, 170, 238, 255);
        public Color textColorWhite = Color.white;
        public Color textColorBlack = new Color32(68, 68, 102, 255);
        Color textColor;


        private static GlobalColor instance;
        public static GlobalColor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GlobalColor>();

                    if (instance == null)
                    {
                        // 如果场景中不存在实例，则报错
                        Debug.LogError("不存在GlobalColor实例，请创建一个gameobject并挂载GlobalColor脚本");
                    }
                }

                return instance;
            }
        }

        public static event Action<Color> OnThemeColorChange;

        public static Color ThemeColor => Instance.themeColor;
        public static Color TextColorWhite => Instance.textColorWhite;
        public static Color TextColorBlack => Instance.textColorBlack;
        public static Color TextColor => Instance.textColor;

        public static void SetThemeColor(Color themeColor) 
        {
            Instance.themeColor = themeColor;
            Instance.textColor = APCA.GetBlackOrWhite(themeColor, TextColorWhite, TextColorBlack);
            OnThemeColorChange?.Invoke(themeColor);
        }
    }
}