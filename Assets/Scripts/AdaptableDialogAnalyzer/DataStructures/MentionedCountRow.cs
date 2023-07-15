using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 提及次数统计中的一行，保存此角色提及其他角色的次数
    /// </summary>
    [Serializable]
    public class MentionedCountRow
    {
        public int speakerId;
        /// <summary>
        /// 此角色提及其他角色的次数
        /// </summary>
        public List<MentionedCountGrid> mentionedCountGrids = new List<MentionedCountGrid>();

        /// <summary>
        /// 此角色的台词统计
        /// </summary>
        public int serifCount = 0;

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
                    MentionedCountGrid mentionedCountGrid = this[character.id];
                    if (mentionedCountGrid != null && mentionedCountGrid.Count > 0)
                    {
                        vector2Ints.Add(new Vector2Int(mentionedCountGrid.mentionedPersonId, mentionedCountGrid.Count));
                    }
                }
                return vector2Ints.ToArray();
            }
        }

        /// <summary>
        /// 如果不存在单元则返回null 
        /// </summary>
        public MentionedCountGrid this[int mentionedPersonId]
        {
            get
            {
                foreach (var mentionedCountGrid in mentionedCountGrids)
                {
                    if (mentionedCountGrid.mentionedPersonId == mentionedPersonId) return mentionedCountGrid;
                }
                return null;
            }
        }

        public MentionedCountRow(int speakerId)
        {
            this.speakerId = speakerId;
        }

        /// <summary>
        /// 请通过此方法添加匹配到的对话 
        /// </summary>
        public void AddMatchedDialogue(int mentionedPersonId, int refIdx)
        {
            if (this[mentionedPersonId] == null)
            {
                mentionedCountGrids.Add(new MentionedCountGrid(mentionedPersonId));
            }
            this[mentionedPersonId].AddMatchedDialogue(refIdx);
        }

        /// <summary>
        /// 清除为空的单元，保存前调用
        /// </summary>
        public void RemoveEmptyGrids()
        {
            List<MentionedCountGrid> removeGrids = mentionedCountGrids
                .Where(g => g == null || g.Count == 0)
                .ToList();
            mentionedCountGrids.RemoveAll(g=>removeGrids.Contains(g));
        }
    }
}