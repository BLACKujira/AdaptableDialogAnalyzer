using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/ColorList")]
    public class ColorList : ScriptableObject
    {
        public List<Color> colors;

        public Color this[int index] => colors[index];
    }
}