using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一篇剧情中提到{对象}的次数
    /// </summary>
    [Serializable]
    public class ObjectMentionedCountRow
    {
        /// <summary>
        /// 当此项不为零时， Count属性返回此项的数值
        /// </summary>
        public int overrideCount = 0;

        public int speakerId;
        public List<int> matchedIndexes = new List<int>();

        /// <summary>
        /// 匹配到的次数统计，当overrideCount不为0时返回overrideCount，否则等同于matched.Count
        /// </summary>
        public int Count => overrideCount <= 0 ? matchedIndexes.Count : overrideCount;

        /// <summary>
        /// 获取匹配到所有ID的集合 
        /// </summary>
        public HashSet<int> MatchedIndexSet
        {
            get
            {
                HashSet<int> matchedIndexes = new HashSet<int>(this.matchedIndexes);
                return matchedIndexes;
            }
        }

        /// <summary>
        /// 判断某句台词是否被匹配
        /// </summary>
        public bool HasSerif(int refIdx)
        {
            return matchedIndexes.Contains(refIdx);
        }

        /// <summary>
        /// 此角色的台词统计
        /// </summary>
        public int serifCount = 0;

        /// <summary>
        /// 添加匹配项，不会重复添加相同的，添加成功返回true
        /// </summary>
        public bool AddMatchedDialogue(int refIdx)
        {
            if (HasSerif(refIdx)) return false;
            matchedIndexes.Add(refIdx);

            return true;
        }

        /// <summary>
        /// 添加多个匹配项，不会重复添加相同的
        /// </summary>
        public void AddMatchedDialogues(int[] refIdxes)
        {
            foreach (var refIdex in refIdxes)
            {
                AddMatchedDialogue(refIdex);
            }
        }

        /// <summary>
        /// 移除匹配项，没有则返回false，移除成功返回true
        /// </summary>
        public bool RemoveMatchedDialogue(int refIdx)
        {
            return matchedIndexes.Remove(refIdx);
        }

        public ObjectMentionedCountRow(int speakerId)
        {
            this.speakerId = speakerId;
        }
    }
}