using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 统计和管理所有剧情的统计结果
    /// </summary>
    public class MentionedCountManager
    {
        public List<MentionedCountMatrix> mentionedCountMatrices = new List<MentionedCountMatrix>();

        public CharacterMentionStats this[int speakerId, int mentionedPersonId] => new CharacterMentionStats(mentionedCountMatrices.ToArray(), speakerId, mentionedPersonId);
    }
}