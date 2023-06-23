using AdaptableDialogAnalyzer.Games.YuRis;
using System.Collections.Generic;
using System.IO;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class Kimihane_LabelInfoLoader : MonoBehaviour
    {
        public string yslFilePath;

        const int startIndex = 1036;

        Dictionary<string,int> labelYbnDictionary = new Dictionary<string,int>();

        List<YSLElement> elements = null;
        public List<YSLElement> Elements
        {
            get
            {
                if(elements == null) Initialize();
                return elements;
            }
        }

        /// <summary>
        /// 获取某一个跳转标签所在的YbnId，-1为标签不存在
        /// </summary>
        public int GetContainYbn(string label)
        {
            if (elements == null) Initialize();

            if (labelYbnDictionary.ContainsKey(label)) return labelYbnDictionary[label];
            return -1;
        }

        /// <summary>
        /// 如label以TO_开头，则改为判断不含TO_的标签所在文件
        /// </summary>
        public int GetContainYbnAutoJump(string label)
        {
            int tryGetYbnId = -1;
            if(label.StartsWith("TO_"))
            {
                tryGetYbnId = GetContainYbn(label.Substring(3));
            }
            
            if(tryGetYbnId != -1) return tryGetYbnId;

            return GetContainYbn(label);
        }

        private void Initialize()
        {
            byte[] bytes = File.ReadAllBytes(yslFilePath);
            elements = KimihaneHelper.GetYSLElements(bytes, startIndex);
            foreach (var element in elements)
            {
                labelYbnDictionary[element.name] = element.ybnId;
            }
        }
    }
}