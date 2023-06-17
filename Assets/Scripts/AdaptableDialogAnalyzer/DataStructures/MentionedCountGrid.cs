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
        /// ƥ�䵽�Ĵ���ͳ�ƣ���ͬ��matched.Count��
        /// </summary>
        public int Count => matchedIndexes.Count;

        /// <summary>
        /// ��ȡƥ�䵽����ID�ļ��� 
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
        /// �ж�ĳ��̨���Ƿ�ƥ��
        /// </summary>
        public bool HasSerif(int refIdx)
        {
            return matchedIndexes.Contains(refIdx);
        }

        /// <summary>
        /// ���ƥ��������ظ������ͬ�ģ���ӳɹ�����true
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
        /// �Ƴ�ƥ���û���򷵻�false���Ƴ��ɹ�����true
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