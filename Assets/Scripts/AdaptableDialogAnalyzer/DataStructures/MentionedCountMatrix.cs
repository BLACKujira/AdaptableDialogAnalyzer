using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private bool hasChanged = false;

        /// <summary>
        /// 统计的故事章节，运行时由MentionedCountResultLoader获取，不参与序列化
        /// </summary>
        private Chapter chapter;

        /// <summary>
        /// 序列化一部分章节的信息，以便不读取章节的情况下使用某些功能
        /// </summary>
        public ChapterInfo chapterInfo;

        /// <summary>
        /// 每个角色提到每个角色的次数
        /// </summary>
        public MentionedCountRow[] mentionedCountRows;

        /// <summary>
        /// 此剧情中指代不明的提及
        /// </summary>
        public List<UnidentifiedMentions> unidentifiedMentionsList = new List<UnidentifiedMentions>();

        public Chapter Chapter
        {
            get
            {
                if (chapter == null) throw new Exception("未加载章节，请使用MentionCountEditor加载统计数据，或在代码中设置此属性");
                return chapter;
            }
            set => chapter = value;
        }
        public bool HasChanged { get => hasChanged; set => hasChanged = value; }

        public MentionedCountRow this[int speakerId] => mentionedCountRows[speakerId];
        public MentionedCountGrid this[int speakerId, int mentionedPersonId] => mentionedCountRows[speakerId][mentionedPersonId];

        public MentionedCountMatrix(Chapter chapter, int size)
        {
            this.chapter = chapter;
            chapterInfo = new ChapterInfo(chapter);

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
        public bool AddUnidentifiedSerif(string unidentifiedNickname, int refIdx)
        {
            UnidentifiedMentions unidentifiedMentions;
            unidentifiedMentions = GetUnidentifiedMentions(unidentifiedNickname);
            if (unidentifiedMentions == null)
            {
                unidentifiedMentions = new UnidentifiedMentions(unidentifiedNickname);
                unidentifiedMentionsList.Add(unidentifiedMentions);
            }
            return unidentifiedMentions.AddMatchedDialogue(refIdx);
        }

        /// <summary>
        /// 返回每句台词的统计信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,List<int>> GetSnippetCountDictionary()
        {
            Dictionary<int, List<int>> snippetCountDictionary = new Dictionary<int, List<int>>();
            BasicTalkSnippet[] basicTalkSnippets = chapter.GetTalkSnippets();
            foreach (var basicTalkSnippet in basicTalkSnippets)
            {
                snippetCountDictionary[basicTalkSnippet.RefIdx] = new List<int>();
            }

            int size = GlobalConfig.CharacterDefinition.characters.Count;
            for (int speakerId = 0; speakerId < size; speakerId++)
            {
                for (int mentionedPersonId = 0; mentionedPersonId < size; mentionedPersonId++)
                {
                    MentionedCountGrid mentionedCountGrid = this[speakerId, mentionedPersonId];
                    foreach (var refIdx in mentionedCountGrid.matchedIndexes)
                    {
                        snippetCountDictionary[refIdx].Add(mentionedPersonId);
                    }
                }
            }

            return snippetCountDictionary;
        }

        /// <summary>
        /// 某句是否含有任一多义昵称
        /// </summary>
        public bool HasUnidentifiedMention(int refIdx)
        {
            foreach (var unidentifiedMentions in unidentifiedMentionsList)
            {
                foreach (var id in unidentifiedMentions.matchedIndexes)
                {
                    if(id == refIdx) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回每个角色台词的合计（不用获取TalkSnippets的情况下获取台词数） 
        /// </summary>
        public int GetSerifCount()
        {
            return mentionedCountRows.Sum(r => r.SerifCount);
        }
    }
}