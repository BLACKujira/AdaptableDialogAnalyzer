using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一句台词中提到{对象}的次数，一句台词中可以有多个匹配
    /// </summary>
    [Serializable]
    public class ObjectMentionedCountMutiGrid
    {
        /// <summary>
        /// 当此项不为零时， Count属性返回此项的数值
        /// </summary>
        public int overrideCount = 0;

        public int refIdx;
        public List<RegexCapture> regexCaptures = new List<RegexCapture>();

        /// <summary>
        /// 匹配到的次数统计，当overrideCount不为0时返回overrideCount，否则等同于matched.Count
        /// </summary>
        public int Count => overrideCount <= 0 ? regexCaptures.Count : overrideCount;

        /// <summary>
        /// 添加匹配项，不会重复添加相同的，添加成功返回true
        /// </summary>
        public bool AddRegexCapture(RegexCapture regexCapture)
        {
            foreach(var capture in regexCaptures)
            {
                if(capture.Equals(regexCapture))
                {
                    return false;
                }
            }
            regexCaptures.Add(regexCapture);
            return true;
        }

        /// <summary>
        /// 添加多个匹配项，不会重复添加相同的
        /// </summary>
        public void AddRegexCaptures(RegexCapture[] regexCaptures)
        {
            foreach (var capture in regexCaptures)
            {
                AddRegexCapture(capture);
            }
        }

        public ObjectMentionedCountMutiGrid(int refIdx)
        {
            this.refIdx = refIdx;
        }
    }
}