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
                for (int i = 0; i < GlobalConfig.CharacterDefinition.characters.Count; i++)
                {
                    if (mentionedCountGrids[i] != null && mentionedCountGrids[i].Count > 0)
                    {
                        vector2Ints.Add(new Vector2Int(i, mentionedCountGrids[i].Count));
                    }
                }
                return vector2Ints.ToArray();
            }
        }

        public MentionedCountGrid this[int mentionedPersonId] => mentionedCountGrids[mentionedPersonId];

        public MentionedCountRow(int size)
        {
            mentionedCountGrids = new MentionedCountGrid[size];
            for (int i = 0; i < size; i++)
            {
                mentionedCountGrids[i] = new MentionedCountGrid();
            }
        }
    }
}