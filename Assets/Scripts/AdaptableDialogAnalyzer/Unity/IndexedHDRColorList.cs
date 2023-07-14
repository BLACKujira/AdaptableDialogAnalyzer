using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/IndexedHDRColorList")]
    public class IndexedHDRColorList : ScriptableObject
    {
        public List<IndexedHDRColor> indexedColors = new List<IndexedHDRColor>();
        public Color defaultColor = Color.white;

        /// <summary>
        /// 不存在时返回默认颜色
        /// </summary>
        public Color this[int colorId]
        {
            get
            {
                Color? color = TryGetColor(colorId);
                if (color == null)
                {
                    color = defaultColor;
                    Debug.LogWarning($"不存在id为{color}的color");
                }
                return (Color)color;
            }
        }

        /// <summary>
        /// 当不存在时返回null
        /// </summary>
        /// <param name="spriteId"></param>
        /// <returns></returns>
        public Color? TryGetColor(int colorId)
        {
            foreach (var indexedColor in indexedColors)
            {
                if (indexedColor.id == colorId) return indexedColor.color;
            }
            return null;
        }
    }
}