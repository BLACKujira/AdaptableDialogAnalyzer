using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 统计和管理所有剧情的统计结果
    /// </summary>
    [Serializable]
    public class ObjectMentionedCountMutiManager : CountManager
    {
        public List<ObjectMentionedCountMutiMatrix> mentionedCountMatrices = new List<ObjectMentionedCountMutiMatrix>();

        public override CountMatrix[] CountMatrices => mentionedCountMatrices.ToArray();

        public Dictionary<int, int> MentionedCountDictionary
        {
            get
            {
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                foreach (var mentionedCountMatrix in mentionedCountMatrices)
                {
                    Vector2Int[] mentionedCountArray = mentionedCountMatrix.MentionedCountArray;
                    foreach (var vector2 in mentionedCountArray)
                    {
                        if (!dictionary.ContainsKey(vector2.x)) dictionary[vector2.x] = 0;
                        dictionary[vector2.x] += vector2.y;
                    }
                }
                return dictionary;
            }
        }

        /// <summary>
        /// 统计角色的台词数量
        /// </summary>
        /// <param name="characterId"></param>
        public int CountSerif(int characterId)
        {
            return mentionedCountMatrices.Sum(m => m[characterId]?.serifCount ?? 0);
        }
    }
}