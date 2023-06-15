using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 在主题色改变时自动改变颜色
    /// </summary>
    public class ThemeColorSyncer : IndividualColorElement
    {
        bool reverse = false;

        private void OnEnable()
        {
            SetIndividualColor(GlobalColor.ThemeColor, reverse, false);
            GlobalColor.OnThemeColorChange += SyncThemeColor;
        }

        private void OnDisable()
        {
            GlobalColor.OnThemeColorChange -= SyncThemeColor;
        }

        private void SyncThemeColor(Color themeColor)
        {
            SetIndividualColor(themeColor, reverse, true);
        }
    }
}