using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一篇剧情中提到{对象}的次数
    /// </summary>
    [Serializable]
    public class ObjectMentionedCountMatrix : CountMatrix
    {
        /// <summary>
        /// 每个角色提到{对象}的次数
        /// </summary>
        public List<ObjectMentionedCountRow> mentionedCountRows = new List<ObjectMentionedCountRow>();

        /// <summary>
        /// 此剧情中需要手动判断的提及
        /// </summary>
        public ObjectMentionedCountRow unidentifiedMentionsRow = new ObjectMentionedCountRow(0);

        /// <summary>
        /// 所有角色提到{对象}的次数的合计
        /// </summary>
        public int Total => mentionedCountRows.Sum(row => row.Count);

        /// <summary>
        /// 以x：角色ID，y：提及次数 的形式返回统计数据
        /// </summary>
        public Vector2Int[] MentionedCountArray
        {
            get
            {
                List<Vector2Int> vector2Ints = new List<Vector2Int>();
                Character[] characters = GlobalConfig.CharacterDefinition.Characters;
                foreach (Character character in characters)
                {
                    ObjectMentionedCountRow mentionedCountRow = this[character.id];
                    if (mentionedCountRow != null && mentionedCountRow.Count > 0)
                    {
                        vector2Ints.Add(new Vector2Int(mentionedCountRow.speakerId, mentionedCountRow.Count));
                    }
                }
                return vector2Ints.ToArray();
            }
        }

        /// <summary>
        /// 所有匹配到的对话ID
        /// </summary>
        public HashSet<int> MatchedRefIdxSet => new HashSet<int>(mentionedCountRows.SelectMany(r => r.matchedIndexes));

        /// <summary>
        /// 是否没有任何一句台词匹配
        /// </summary>
        public bool NoMatch
        {
            get
            {
                if (mentionedCountRows == null || mentionedCountRows.Count == 0) return true;
                foreach (var row in mentionedCountRows)
                {
                    if (row.Count > 0) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 是否不存在有歧义的匹配
        /// </summary>
        public bool NoUnidentifiedMatch => unidentifiedMentionsRow.Count == 0;

        /// <summary>
        /// 是否匹配到某句台词
        /// </summary>
        public bool HasMatched(int refIdx)
        {
            foreach (var mentionedCountRow in mentionedCountRows)
            {
                if(mentionedCountRow.HasSerif(refIdx)) return true;
            }
            return false;
        }

        /// <summary>
        /// 如果不存在行则返回null 
        /// </summary>
        public ObjectMentionedCountRow this[int speakerId]
        {
            get
            {
                foreach (var mentionedCountRow in mentionedCountRows)
                {
                    if (mentionedCountRow.speakerId == speakerId) return mentionedCountRow;
                }
                return null;
            }
        }

        /// <summary>
        /// 请通过此方法添加匹配到的对话 
        /// </summary>
        public void AddMatchedDialogue(int speakerId, int refIdx)
        {
            if (this[speakerId] == null)
            {
                mentionedCountRows.Add(new ObjectMentionedCountRow(speakerId));
            }
            this[speakerId].AddMatchedDialogue(refIdx);
        }

        /// <summary>
        /// 增加角色台词数的统计 
        /// </summary>
        public void AddSerifCount(int speakerId, int count)
        {
            if (this[speakerId] == null)
            {
                mentionedCountRows.Add(new ObjectMentionedCountRow(speakerId));
            }
            this[speakerId].serifCount += count;
        }

        /// <summary>
        /// 清除为空的行，保存前调用
        /// </summary>
        public void RemoveEmptyRows()
        {
            List<ObjectMentionedCountRow> removeRows = mentionedCountRows
                .Where(r => r == null || (r.Count == 0 && r.serifCount == 0))
                .ToList();
            mentionedCountRows.RemoveAll(r => removeRows.Contains(r));
        }

        /// <summary>
        /// 序列化并保存文件到 filePath
        /// </summary>
        public void SerializeAndSave(string filePath)
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(filePath, json);
        }

        public static ObjectMentionedCountMatrix LoadAndDeserialize(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<ObjectMentionedCountMatrix>(json);
        }

        public ObjectMentionedCountMatrix(Chapter chapter) : base(chapter) { }

        /// <summary>
        /// 返回每个角色台词的合计（不用获取TalkSnippets的情况下获取台词数） 
        /// </summary>
        public int GetSerifCount()
        {
            return mentionedCountRows.Sum(r => r.serifCount);
        }
    }
}