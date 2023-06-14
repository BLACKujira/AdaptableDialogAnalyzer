using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一篇剧情中角色A提到角色B的次数
    /// </summary>
    [Serializable]
    public class MentionedCountMatrix
    {
        /// <summary>
        /// 在编辑器中使用，当修改后变成True
        /// </summary>
        [NonSerialized] public bool hasChanged = false;

        /// <summary>
        /// 统计的故事章节，运行时获取，不参与序列化
        /// </summary>
        [NonSerialized] public Chapter chapter;

        /// <summary>
        /// 每个角色提到每个角色的次数
        /// </summary>
        public MentionedCountRow[] mentionedCountRows;

        /// <summary>
        /// 此剧情中指代不明的提及
        /// </summary>
        public List<UnidentifiedMentions> unidentifiedMentionsList = new List<UnidentifiedMentions>();

        public MentionedCountRow this[int speakerId] => mentionedCountRows[speakerId];
        public MentionedCountGrid this[int speakerId, int mentionedPersonId] => mentionedCountRows[speakerId][mentionedPersonId];

        public MentionedCountMatrix(int size)
        {
            mentionedCountRows = new MentionedCountRow[size];
            for (int i = 0; i < size; i++)
            {
                mentionedCountRows[i] = new MentionedCountRow(size);
            }
        }

        /// <summary>
        /// 这段剧情中是否匹配到了模糊昵称，若匹配成功则返回其对象
        /// </summary>
        public UnidentifiedMentions GetUnidentifiedMentions(string unidentifiedNickname)
        {
            foreach (var unidentifiedMentions in unidentifiedMentionsList)
            {
                if (unidentifiedMentions.unidentifiedNickname.Equals(unidentifiedNickname))
                    return unidentifiedMentions;
            }
            return null;
        }

        /// <summary>
        /// 添加模糊昵称的匹配结果
        /// </summary>
        public bool AddUnidentifiedSerif(string unidentifiedNickname, int refIdx, int startIndex, int length)
        {
            UnidentifiedMentions unidentifiedMentions;
            unidentifiedMentions = GetUnidentifiedMentions(unidentifiedNickname);
            if (unidentifiedMentions == null)
            {
                unidentifiedMentions = new UnidentifiedMentions(unidentifiedNickname);
                unidentifiedMentionsList.Add(unidentifiedMentions);
            }
            return unidentifiedMentions.AddMatchedDialogue(refIdx, startIndex, length);
        }
    }
}