﻿using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 所有剧情中一名角色提到另一名角色的次数
    /// </summary>
    public class CharacterMentionStats
    {
        /// <summary>
        /// 分类别的统计结果，key：剧情类型，value：提及此时
        /// </summary>
        public Dictionary<string, int> countDictionary = new Dictionary<string, int>();

        /// <summary>
        /// 获取某一类别的统计次数
        /// </summary>
        /// <param name="chapterType"></param>
        /// <returns></returns>
        public int this[string chapterType]
        {
            get
            {
                if (!countDictionary.ContainsKey(chapterType)) return 0;
                else return countDictionary[chapterType];
            }
        }

        int speakerId;
        int mentionedPersonId;

        public int SpeakerId => speakerId;
        public int MentionedPersonId => mentionedPersonId;

        public CharacterMentionStats(MentionedCountMatrix[] mentionedCountMatrices, int speakerId, int mentionedPersonId)
        {
            this.speakerId = speakerId;
            this.mentionedPersonId = mentionedPersonId;

            foreach (var mentionedCountMatrix in mentionedCountMatrices)
            {
                string chapterType;
                if (mentionedCountMatrix.TryGetChapter() != null && !string.IsNullOrEmpty(mentionedCountMatrix.Chapter.ChapterType))
                {
                    chapterType = mentionedCountMatrix.Chapter.ChapterType;
                }
                else
                {
                    chapterType = "default";
                }
                if (!countDictionary.ContainsKey(chapterType))
                    countDictionary[chapterType] = 0;
                countDictionary[chapterType] += mentionedCountMatrix[speakerId, mentionedPersonId]?.Count ?? 0;
            }
        }

        public int Total
        {
            get
            {
                int total = 0;
                foreach (var keyValuePair in countDictionary)
                {
                    total += keyValuePair.Value;
                }
                return total;
            }
        }
    }
}