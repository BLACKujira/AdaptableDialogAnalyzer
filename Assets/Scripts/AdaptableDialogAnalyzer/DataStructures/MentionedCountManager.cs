using System.Collections.Generic;
using System.Linq;

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
                if(!dictionary.ContainsKey(countMatrix.Chapter.ChapterType))
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
    }
}