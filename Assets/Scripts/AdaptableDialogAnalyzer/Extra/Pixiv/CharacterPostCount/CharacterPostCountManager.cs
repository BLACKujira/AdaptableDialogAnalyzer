using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public enum CountManagerType
    {
        /// <summary>
        /// 每日增量
        /// </summary>
        Delta, 
        /// <summary>
        /// 当日总计
        /// </summary>
        Total
    }

    public class CharacterPostCountManager
    {
        public CountManagerType type;
        public Dictionary<DateTime, CharacterPostCountDay> days = new Dictionary<DateTime, CharacterPostCountDay>();

        public CharacterPostCountManager(CountManagerType type)
        {
            this.type = type;
        }

        /// <summary>
        /// 向某一天添加某一角色的计数
        /// </summary>
        public void Add(DateTime date, int characterId, int count = 1)
        {
            if(!days.ContainsKey(date))
            {
                days.Add(date, new CharacterPostCountDay(date));
            }
            days[date].Add(characterId, count);
        }
        

        /// <summary>
        /// 转化为总计模式
        /// </summary>
        public CharacterPostCountManager ToTotalMode() 
        {
            if(type == CountManagerType.Total)
            {
                Debug.Log("此统计结果已是总计模式");
                return this;
            }

            CharacterPostCountManager characterPostCountManager = new CharacterPostCountManager(CountManagerType.Total);

            Dictionary<int, int> currentCount = new Dictionary<int, int>();
            List<CharacterPostCountDay> characterPostCountDays = days
                .Select(d => d.Value)
                .OrderBy(d => d.date)
                .ToList();

            foreach (var characterPostCountDay in characterPostCountDays)
            {
                foreach (var characterCount in characterPostCountDay.characterCountPairs)
                {
                    if(!currentCount.ContainsKey(characterCount.Key))
                    {
                        currentCount.Add(characterCount.Key, 0);
                    }
                    currentCount[characterCount.Key] += characterCount.Value;
                }

                foreach (var keyValuePair in currentCount)
                {
                    characterPostCountManager.Add(characterPostCountDay.date, keyValuePair.Key, keyValuePair.Value);
                }
            }

            return characterPostCountManager;
        }
    }
}