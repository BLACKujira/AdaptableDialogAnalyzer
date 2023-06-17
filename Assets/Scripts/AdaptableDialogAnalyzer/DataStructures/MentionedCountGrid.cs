using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.DataStructures
{
    [Serializable]
    public class MentionedCountGrid
    {
        public List<int> matchedIndexes = new List<int>();

        /// <summary>
        /// 匹配到的次数统计（等同于matched.Count）
        /// </summary>
        public int Count => matchedIndexes.Count;

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
        /// 添加匹配项，不会重复添加相同的，添加成功返回true
        /// </summary>
        /// <param name="refIdx"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public bool AddMatchedDialogue(int refIdx)
        {
            if (HasSerif(refIdx)) return false;

            matchedIndexes.Add(refIdx);
            return true;
        }

        /// <summary>
        /// 移除匹配项，没有则返回false，移除成功返回true
        /// </summary>
        /// <param name="refIdx"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>

        public bool RemoveMatchedDialogue(int refIdx)
        {
            return matchedIndexes.Remove(refIdx);
        }
    }
}