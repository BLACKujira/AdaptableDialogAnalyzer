using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一篇剧情中提到{对象}的次数，一句台词中可以有多个匹配
    /// </summary>
    [Serializable]
    public class ObjectMentionedCountMutiRow
    {
        /// <summary>
        /// 当此项不为零时， Count属性返回此项的数值
        /// </summary>
        public int overrideCount = 0;

        public int speakerId;
        public List<ObjectMentionedCountMutiGrid> matchedGrids = new List<ObjectMentionedCountMutiGrid>();

        /// <summary>
        /// 匹配到的次数统计，当overrideCount不为0时返回overrideCount，否则等同于matched.Count
        /// </summary>
        public int Count => overrideCount <= 0 ? matchedGrids.Sum(grid => grid.Count) : overrideCount;

        /// <summary>
        /// 获取台词refIdx的匹配对象
        /// </summary>
        public ObjectMentionedCountMutiGrid this[int refIdx] => matchedGrids.Find(grid => grid.refIdx == refIdx);

        /// <summary>
        /// 判断某句台词是否被匹配
        /// </summary>
        public bool HasSerif(int refIdx)
        {
            foreach (var grid in matchedGrids)
            {
                if (grid.refIdx == refIdx) return true;
            }
            return false;
        }

        /// <summary>
        /// 此角色的台词统计
        /// </summary>
        public int serifCount = 0;

        /// <summary>
        /// 添加匹配项，不会重复添加相同的，添加成功返回true
        /// </summary>
        public bool AddRegexCapture(int refIdx, RegexCapture regexCapture)
        {
            if (this[refIdx] == null)
            {
                ObjectMentionedCountMutiGrid objectMentionedCountMutiGrid = new ObjectMentionedCountMutiGrid(refIdx);
                matchedGrids.Add(objectMentionedCountMutiGrid);
            }
            return this[refIdx].AddRegexCapture(regexCapture);
        }

        /// <summary>
        /// 添加匹配项，不会重复添加相同的，添加成功返回true
        /// </summary>
        public bool AddRegexCapture(int refIdx, Capture capture)
        {
            return AddRegexCapture(refIdx, RegexCapture.FromCapture(capture));
        }

        /// <summary>
        /// 添加多个匹配项，不会重复添加相同的
        /// </summary>
        public void AddRegexCaptures(int refIdx, RegexCapture[] regexCaptures)
        {
            foreach (var capture in regexCaptures)
            {
                AddRegexCapture(refIdx, capture);
            }
        }

        /// <summary>
        /// 移除一句台词的所有匹配
        /// </summary>
        public bool RemoveGrid(int refIdx)
        {
            ObjectMentionedCountMutiGrid objectMentionedCountMutiGrid = this[refIdx];
            if(objectMentionedCountMutiGrid == null) return false;

            matchedGrids.Remove(objectMentionedCountMutiGrid);
            return true;
        }

        public ObjectMentionedCountMutiRow(int speakerId)
        {
            this.speakerId = speakerId;
        }
    }
}