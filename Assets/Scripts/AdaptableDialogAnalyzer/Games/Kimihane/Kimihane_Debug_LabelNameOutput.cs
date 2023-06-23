using AdaptableDialogAnalyzer.Games.YuRis;
using AdaptableDialogAnalyzer.Games.YuRis.ExtYbn;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    /// <summary>
    /// 输出ysl文件中跳转点的信息
    /// </summary>
    public class Kimihane_Debug_LabelNameOutput : MonoBehaviour
    {
        public int startIndex = 1036;
        public TextAsset yslAsset;
        [Header("Extra")]
        public int ybnId = -1;
        public string contains;

        public void Start()
        {
            byte[] ysl = yslAsset.bytes;

            List<YSLElement> elements = KimihaneHelper.GetYSLElements(ysl, startIndex);

            if (ybnId >= 0) elements = elements.Where(elem => elem.ybnId == ybnId).ToList();
            if (!string.IsNullOrEmpty(contains)) elements = elements.Where(elem => elem.name.Contains(contains)).ToList();

            elements = elements.OrderBy(elem => elem.ybnId).ThenBy(elem => elem.value2).ToList();

            foreach (var element in elements)
            {
                Debug.Log($"{element.ybnId:000} {element.name} {element.value1} {element.value2} {element.value4}");
            }
        }
    }
}