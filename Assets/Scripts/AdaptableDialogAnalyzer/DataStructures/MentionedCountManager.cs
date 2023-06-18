using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 统计和管理所有剧情的统计结果
    /// </summary>
    public class MentionedCountManager
    {
        public List<MentionedCountMatrix> mentionedCountMatrices = new List<MentionedCountMatrix>();

        public CharacterMentionStats this[int speakerId, int mentionedPersonId] => new CharacterMentionStats(mentionedCountMatrices.ToArray(), speakerId, mentionedPersonId);

        /// <summary>
        /// 获取以剧情类型分类的统计矩阵 
        /// </summary>
        /// <returns>以剧情类型为键，对应类型的统计矩阵列表为值的字典</returns>
        public Dictionary<string, List<MentionedCountMatrix>> GetCountMatrixByType()
        {
            Dictionary<string, List<MentionedCountMatrix>> dictionary = new Dictionary<string, List<MentionedCountMatrix>>();
            foreach (var countMatrix in mentionedCountMatrices)
            {
                if (!dictionary.ContainsKey(countMatrix.Chapter.ChapterType))
                {
                    dictionary[countMatrix.Chapter.ChapterType] = new List<MentionedCountMatrix>();
                }
                dictionary[countMatrix.Chapter.ChapterType].Add(countMatrix);
            }
            return dictionary;
        }

        /// <summary>
        /// 统计角色的台词数量
        /// </summary>
        /// <param name="characterId"></param>
        public int CountSerif(int characterId)
        {
            return mentionedCountMatrices.Sum(m => m[characterId].SerifCount);
        }

        /// <summary>
        /// 获取包含多义昵称匹配的统计矩阵
        /// </summary>
        /// <returns>以多义昵称为键，统计矩阵列表为值的字典</returns>
        public Dictionary<string, List<MentionedCountMatrix>> GetMatricesWithUnidentifiedMatches()
        {
            Dictionary<string, List<MentionedCountMatrix>> dictionary = new Dictionary<string, List<MentionedCountMatrix>>();
            foreach (var mentionedCountMatrix in mentionedCountMatrices)
            {
                foreach (var unidentifiedMentions in mentionedCountMatrix.unidentifiedMentionsList)
                {
                    if (!dictionary.ContainsKey(unidentifiedMentions.unidentifiedNickname))
                    {
                        dictionary[unidentifiedMentions.unidentifiedNickname] = new List<MentionedCountMatrix>();
                    }
                    dictionary[unidentifiedMentions.unidentifiedNickname].Add(mentionedCountMatrix);
                }
            }
            return dictionary;
        }


        /// <summary>
        /// 判断相差过大的组合的函数
        /// key：输入为较大一方，较小一方（顺序固定）输出为是否相差过大的函数
        /// value：原因（函数功能的自然语言表述）
        /// </summary>
        private readonly (Func<int, int, bool> func, string reason)[] discrepancyCheckFunctions = new (Func<int, int, bool>, string)[]
        {
            ( DiscrepancyCheck_3Multiples, "两方均大于10，\n但一方大于另一方的三倍" ),
            ( DiscrepancyCheck_Missing, "一方小于3，\n而另一方大于10" )
        };

        /// <summary>
        /// 获取存在相差过大的角色组合
        /// </summary>
        /// <returns>字典，键为数字较大的角色，值为数字较小的角色和原因的组合</returns>
        public Dictionary<Vector2Int, string> GetDiscrepancyPairs()
        {
            Dictionary<Vector2Int, string> dictionary = new Dictionary<Vector2Int, string>();

            int size = GlobalConfig.CharacterDefinition.characters.Count;
            for (int charAId = 0; charAId < size; charAId++)
            {
                for (int charBId = charAId + 1; charBId < size; charBId++)
                {
                    CharacterMentionStats bigStats = this[charAId, charBId];
                    CharacterMentionStats smallStats = this[charBId, charAId];
                    int big = bigStats.Total;
                    int small = smallStats.Total;
                    if (big < small)
                    {
                        (bigStats, smallStats) = (smallStats, bigStats);
                        (big, small) = (small, big);
                    }

                    foreach (var kvp in discrepancyCheckFunctions)
                    {
                        if(kvp.func(big, small))
                        {
                            dictionary[new Vector2Int(bigStats.SpeakerId, bigStats.MentionedPersonId)] = kvp.reason;
                            break;
                        }
                    }
                }
            }

            return dictionary;
        }

        private static bool DiscrepancyCheck_3Multiples(int big, int small)
        {
            if (big < 10 || small < 10) return false;
            return small * 3 < big;
        }
        private static bool DiscrepancyCheck_Missing(int big, int small)
        {
            return big > 10 && small < 3;
        }
    }
}