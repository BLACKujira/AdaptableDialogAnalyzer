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
        /// �ж�ĳ��̨���Ƿ�ƥ��
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
        /// ���ƥ��������ظ������ͬ�ģ���ӳɹ�����true
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