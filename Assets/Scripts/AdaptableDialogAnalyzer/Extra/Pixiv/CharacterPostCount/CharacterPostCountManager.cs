using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class CharacterPostCountManager
    {
        public CountManagerType type;
        public Dictionary<DateTime, CharacterPostCountDay> days = new Dictionary<DateTime, CharacterPostCountDay>();

        public Dictionary<int, DateTime> GetDateTimeIndexes()
        {
            Dictionary<int, DateTime> result = new Dictionary<int, DateTime>();
            int index = 0;
            var sortedDays = days.OrderBy(d => d.Key);
            foreach (var day in sortedDays)
            {
                result[index] = day.Key;
                index++;
            }
            return result;
        }

        public CharacterPostCountManager(CountManagerType type)
        {
            this.type = type;
        }

        /// <summary>
        /// 向某一天添加某一角色的计数
        /// </summary>
        public void Add(DateTime date, int characterId, bool isNsfw, int count = 1)
        {
            if(!days.ContainsKey(date))
            {
                days.Add(date, new CharacterPostCountDay(date));
            }
            days[date].Add(characterId, isNsfw , count);
        }

        public void AddOrReplace(DateTime date, CharacterPostCountDayItem characterPostCountDayItem)
        {
            if (!days.ContainsKey(date))
            {
                days.Add(date, new CharacterPostCountDay(date));
            }
            days[date].AddOrReplace(characterPostCountDayItem);
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

            Dictionary<int, CharacterPostCountDayItem> currentCount = new Dictionary<int, CharacterPostCountDayItem>();
            List<CharacterPostCountDay> characterPostCountDays = days
                .Select(d => d.Value)
                .OrderBy(d => d.date)
                .ToList();

            foreach (var characterPostCountDay in characterPostCountDays)
            {
                foreach (var characterCount in characterPostCountDay.characterTotalPairs)
                {
                    if(!currentCount.ContainsKey(characterCount.Key))
                    {
                        currentCount[characterCount.Key] = new CharacterPostCountDayItem(characterCount.Key);
                    }
                    currentCount[characterCount.Key] += characterCount.Value;
                }

                foreach (var keyValuePair in currentCount)
                {
                    characterPostCountManager.AddOrReplace(characterPostCountDay.date, keyValuePair.Value);
                }
            }

            return characterPostCountManager;
        }

        public void FillEmpty(bool beforeAppear = false)
        {
            HashSet<int> appearedCharacterId = new HashSet<int>();
            if (beforeAppear)
            {
                appearedCharacterId.AddAll(Unity.GlobalConfig.CharacterDefinition.Characters
                    .Select(c => c.id)
                    .ToArray());
            }

            foreach (var day in days)
            {
                foreach (var characterId in appearedCharacterId)
                {
                    day.Value.Add(characterId, false, 0);
                }
                appearedCharacterId.AddAll(day.Value.characterTotalPairs
                    .Where(kvp => kvp.Value.total > 0)
                    .Select(kvp => kvp.Value.characterId)
                    .ToArray());
            }
        }
    }
}