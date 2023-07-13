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

        public MentionedCountRow this[int speakerId]
        {
            get
            {
                foreach (var mentionedCountRow in mentionedCountRows)
                {
                    if (mentionedCountRow.speakerId == speakerId) return mentionedCountRow;
                }
                throw new Exception($"此统计矩阵中找不到角色{speakerId}的定义，暂不支持统计后修改角色定义。或者检查是否使用了错误的角色定义");
            }
        }

        public MentionedCountGrid this[int speakerId, int mentionedPersonId] => this[speakerId][mentionedPersonId];

        public MentionedCountMatrix(Chapter chapter)
        {
            this.chapter = chapter;
            chapterInfo = new ChapterInfo(chapter);

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            int size = characters.Length;

            mentionedCountRows = new MentionedCountRow[size];
            for (int i = 0; i < size; i++)
            {
                int characterId = characters[i].id;
                mentionedCountRows[i] = new MentionedCountRow(characterId);
            }
        }

        public MentionedCountMatrix()
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            int size = characters.Length;

            mentionedCountRows = new MentionedCountRow[size];
            for (int i = 0; i < size; i++)
            {
                int characterId = characters[i].id;
                mentionedCountRows[i] = new MentionedCountRow(characterId);
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
        public Dictionary<int, List<int>> GetSnippetCountDictionary()
        {
            Dictionary<int, List<int>> snippetCountDictionary = new Dictionary<int, List<int>>();
            BasicTalkSnippet[] basicTalkSnippets = chapter.TalkSnippets;
            foreach (var basicTalkSnippet in basicTalkSnippets)
            {
                snippetCountDictionary[basicTalkSnippet.RefIdx] = new List<int>();
            }

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            int size = characters.Length;
            for (int id1 = 0; id1 < size; id1++)
            {
                for (int id2 = 0; id2 < size; id2++)
                {
                    int speakerId = characters[id1].id;
                    int mentionedPersonId = characters[id2].id;

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
                    if (id == refIdx) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 某句是否含有某多义昵称
        /// </summary>
        public bool HasUnidentifiedMention(int refIdx, string unidentifiedNickname)
        {
            UnidentifiedMentions unidentifiedMentions = GetUnidentifiedMentions(unidentifiedNickname);
            if (unidentifiedMentions == null) return false;
            return unidentifiedMentions.HasSerif(refIdx);
        }

        /// <summary>
        /// 返回每个角色台词的合计（不用获取TalkSnippets的情况下获取台词数） 
        /// </summary>
        public int GetSerifCount()
        {
            return mentionedCountRows.Sum(r => r.SerifCount);
        }

        /// <summary>
        /// 移除任一多义昵称匹配列表中的此台词，之后检测并移除空多义昵称列表
        /// </summary>
        /// <returns>移除是否成功</returns>
        public bool RemoveUnidentifiedMention(int refIdx)
        {
            bool success = false;

            foreach (var unidentifiedMentions in unidentifiedMentionsList)
            {
                if (unidentifiedMentions.HasSerif(refIdx))
                {
                    unidentifiedMentions.RemoveMatchedDialogue(refIdx);
                    success = true;
                }
            }

            RemoveEmptyUnidentifiedMentions();
            return success;
        }

        /// <summary>
        /// 移除某多义昵称匹配列表中的此台词，之后检测并移除空多义昵称列表
        /// </summary>
        /// <returns>移除是否成功</returns>
        public bool RemoveUnidentifiedMention(string unidentifiedNickname, int refIdx)
        {
            bool success = false;

            UnidentifiedMentions unidentifiedMentions = unidentifiedMentionsList
                .Where(l => l.unidentifiedNickname.Equals(unidentifiedNickname))
                .FirstOrDefault();

            if (unidentifiedMentions != null)
            {
                if (unidentifiedMentions.HasSerif(refIdx))
                {
                    unidentifiedMentions.RemoveMatchedDialogue(refIdx);
                    success = true;
                }
            }

            RemoveEmptyUnidentifiedMentions();
            return success;
        }

        void RemoveEmptyUnidentifiedMentions()
        {
            UnidentifiedMentions[] removeItems = unidentifiedMentionsList.Where(um => um.Count <= 0).ToArray();
            foreach (var unidentifiedMentions in removeItems)
            {
                unidentifiedMentionsList.Remove(unidentifiedMentions);
            }
        }

        /// <summary>
        /// 获取此统计矩阵对应的剧情，没有加载则返回null 
        /// </summary>
        public Chapter TryGetChapter()
        {
            return chapter;
        }
    }
}