using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    [Serializable]
    public class MentionedCountGrid
    {
        public List<MatchedSerif> matched = new List<MatchedSerif>();
        public int Count => matched.Count;

        /// <summary>
        /// 判断某句台词是否被匹配
        /// </summary>
        /// <param name="refIdx"></param>
        /// <returns></returns>
        public bool HasSerif(int refIdx)
        {
            foreach (var matchedSerif in matched)
            {
                if (matchedSerif.refIdx == refIdx)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 添加匹配项，不会重复添加相同的，添加成功返回true
        /// </summary>
        /// <param name="refIdx"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        public bool AddMatchedDialogue(int refIdx, int startIndex, int length)
        {
            if (HasSerif(refIdx)) return false;

            MatchedSerif matchedDialogue = new MatchedSerif(refIdx, startIndex, length);
            matched.Add(matchedDialogue);
            return true;
        }
    }
}