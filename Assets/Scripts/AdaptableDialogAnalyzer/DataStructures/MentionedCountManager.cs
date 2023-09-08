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
    public class MentionedCountManager : CountManager
    {
        public List<MentionedCountMatrix> mentionedCountMatrices = new List<MentionedCountMatrix>();

        public override CountMatrix[] CountMatrices => mentionedCountMatrices.ToArray();

        public CharacterMentionStats this[int speakerId, int mentionedPersonId] => new CharacterMentionStats(mentionedCountMatrices.ToArray(), speakerId, mentionedPersonId);

        /// <summary>
        /// 统计角色的台词数量
        /// </summary>
        /// <param name="characterId"></param>
        public int CountSerif(int characterId)
        {
            return mentionedCountMatrices.Sum(m => m[characterId]?.serifCount ?? 0);
        }

        /// <summary>
        /// 统计一名角色提到其他所有角色的次数 
        /// </summary>
        public int CountMentionTotal(int speakerId, bool passZero, bool passSelf)
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;

            int count = 0;
            foreach (var character in characters)
            {
                if (character.id == 0 && passZero) continue;
                if (character.id == speakerId && passSelf) continue;

                foreach (var mentionedCountMatrix in mentionedCountMatrices)
                {
                    count += mentionedCountMatrix[speakerId, character.id].Count;
                }
            }

            return count;
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
        public Dictionary<Vector2Int, string> GetDiscrepancyPairs(bool passZero)
        {
            Dictionary<Vector2Int, string> dictionary = new Dictionary<Vector2Int, string>();

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            int size = characters.Length;
            for (int id1 = 0; id1 < size; id1++)
            {
                for (int id2 = id1 + 1; id2 < size; id2++)
                {
                    if (passZero && (id1 == 0 || id2 == 0)) continue;

                    int charAId = characters[id1].id;
                    int charBId = characters[id2].id;

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
                        if (kvp.func(big, small))
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

        /// <summary>
        /// 获取一名角色提及其他角色的信息 
        /// </summary>
        public List<CharacterMentionStats> GetMentionStatsList(int speakerId, bool passZero, bool passSelf)
        {
            List<CharacterMentionStats> characterMentionStatsList = new List<CharacterMentionStats>();

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;
            foreach (Character character in characters)
            {
                if (passZero && character.id == 0) continue;
                if (passSelf && character.id == speakerId) continue;

                CharacterMentionStats characterMentionStats = this[speakerId, character.id];
                characterMentionStatsList.Add(characterMentionStats);
            }

            return characterMentionStatsList;
        }

        /// <summary>
        /// 获取所有角色之间的统计矩阵对(不包括自己对自己)
        /// </summary>
        public List<CharacterMentionStatsPair> GetCharacterMentionStatsPairs(bool passZero)
        {
            List<CharacterMentionStatsPair> characterMentionStatsPairs = new List<CharacterMentionStatsPair>();

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;

            for (int i = 0; i < characters.Length; i++)
            {
                for (int j = i + 1; j < characters.Length; j++)
                {
                    if (passZero && (i == 0 || j == 0)) continue;

                    int characterAId = characters[i].id;
                    int characterBId = characters[j].id;

                    CharacterMentionStatsPair characterMentionStatsPair = new CharacterMentionStatsPair(this[characterAId, characterBId], this[characterBId, characterAId]);
                    characterMentionStatsPairs.Add(characterMentionStatsPair);
                }
            }

            return characterMentionStatsPairs;
        }
    }
}