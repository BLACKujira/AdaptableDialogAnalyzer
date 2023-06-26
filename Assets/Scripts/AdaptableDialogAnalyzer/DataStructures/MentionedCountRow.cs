using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
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
        public MentionedCountGrid[] mentionedCountGrids;

        /// <summary>
        /// 此角色的台词
        /// </summary>
        public List<int> serifs = new List<int>();

        /// <summary>
        /// 此角色的台词统计
        /// </summary>
        public int SerifCount => serifs.Count;

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

        public MentionedCountGrid this[int mentionedPersonId]
        {
            get
            {
                foreach (var mentionedCountGrid in mentionedCountGrids)
                {
                    if (mentionedCountGrid.mentionedPersonId == mentionedPersonId) return mentionedCountGrid;
                }
                throw new Exception($"此统计矩阵行中找不到角色{mentionedPersonId}的定义，暂不支持统计后修改角色定义。或者检查是否使用了错误的角色定义");
            }
        }

        public MentionedCountRow(int speakerId)
        {
            this.speakerId = speakerId;

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            int size = characters.Length;

            mentionedCountGrids = new MentionedCountGrid[size];
            for (int i = 0; i < size; i++)
            {
                int characterId = characters[i].id;
                mentionedCountGrids[i] = new MentionedCountGrid(characterId);
            }
        }
    }
}