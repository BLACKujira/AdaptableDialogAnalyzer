using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    [Serializable]
    public class MentionedCountGrid
    {
        /// <summary>
        /// �����Ϊ��ʱ�� Count���Է��ش������ֵ
        /// </summary>
        public int overrideCount = 0;

        public int mentionedPersonId;
        public List<int> matchedIndexes = new List<int>();

        public MentionedCountGrid(int mentionedPersonId)
        {
            this.mentionedPersonId = mentionedPersonId;
        }

        /// <summary>
        /// ƥ�䵽�Ĵ���ͳ�ƣ���overrideCount��Ϊ0ʱ����overrideCount�������ͬ��matched.Count
        /// </summary>
        public int Count => overrideCount <= 0 ? matchedIndexes.Count : overrideCount;

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
        public bool AddMatchedDialogue(int refIdx)
        {
            if (HasSerif(refIdx)) return false;

            matchedIndexes.Add(refIdx);
            return true;
        }

        /// <summary>
        /// ��Ӷ��ƥ��������ظ������ͬ��
        /// </summary>
        public void AddMatchedDialogues(int[] refIdxes)
        {
            foreach (var refIdex in refIdxes)
            {
                AddMatchedDialogue(refIdex);
            }
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