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

        bool deltaCalculated = false;
        bool deltaOfDeltaCalculated;

        /// <summary>
        /// 增量是否已经计算
        /// </summary>
        public bool DeltaCalculated => deltaCalculated;
        /// <summary>
        /// 增量的增量是否已经计算
        /// </summary>
        public bool DeltaOfDeltaCalculated => deltaOfDeltaCalculated;

        /// <summary>
        /// 增量计算范围
        /// </summary>
        int deltaCalculationRange = -1;
        public int DeltaCalculationRange => deltaCalculationRange;

        /// <summary>
        /// 增量的增量计算范围
        /// </summary>
        int deltaOfDeltaCalculationRange = -1;
        public int DeltaOfDeltaCalculationRange => deltaOfDeltaCalculationRange;

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
            if (!days.ContainsKey(date))
            {
                days.Add(date, new CharacterPostCountDay(date));
            }
            days[date].Add(characterId, isNsfw, count);
        }

        /// <summary>
        /// 向某一天添加某一角色的计数，或者替换原有的计数
        /// </summary>
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
            if (type == CountManagerType.Total)
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
                    if (!currentCount.ContainsKey(characterCount.Key))
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

        /// <summary>
        /// 将之前出现但是没有计数的角色填充为0，如果beforeAppear为true，则填充定义的所有角色
        /// </summary>
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

        /// <summary>
        /// 计算前deltaCalculationRange天的增量
        /// </summary>
        /// <param name="deltaCalculationRange"></param>
        public void CalcDelta(int deltaCalculationRange = 7)
        {
            List<CharacterPostCountDay> characterPostCountDays = days
                .OrderBy(d => d.Key)
                .Select(d => d.Value)
                .ToList();

            for (int i = 0; i < characterPostCountDays.Count; i++)
            {
                CharacterPostCountDay currentDay = characterPostCountDays[i];
                foreach (var characterTotalPair in currentDay.characterTotalPairs)
                {
                    characterTotalPair.Value.deltaCalculationRange = deltaCalculationRange;
                    characterTotalPair.Value.deltaCalculated = true;

                    if (type == CountManagerType.Delta) // 增量模式，将每天的增量设置为前deltaCalculationRange天的总计
                    {
                        for (int j = 0; j <= deltaCalculationRange; j++)
                        {
                            if (i - j < 0) break; // 超出范围
                            CharacterPostCountDay prevDay = characterPostCountDays[i - j];
                            if (!prevDay.characterTotalPairs.ContainsKey(characterTotalPair.Key)) break; // 前j天没有这个角色
                            characterTotalPair.Value.delta += prevDay.characterTotalPairs[characterTotalPair.Key].total;
                        }
                    }
                    else // 总计模式，将每天的增量设置为前deltaCalculationRange天的增量
                    {
                        int prevDayIndex = Math.Max(i - deltaCalculationRange, 0);
                        for (int j = prevDayIndex; j <= i; j++) // 寻找第一个有此角色统计结果的天
                        {
                            CharacterPostCountDay prevDay = characterPostCountDays[j];
                            if (prevDay.characterTotalPairs.ContainsKey(characterTotalPair.Key))
                            {
                                characterTotalPair.Value.delta = characterTotalPair.Value.total - prevDay.characterTotalPairs[characterTotalPair.Key].total;
                                break;
                            }
                        }
                    }
                }
            }

            deltaCalculated = true;
            this.deltaCalculationRange = deltaCalculationRange;
        }

        /// <summary>
        /// 计算前deltaOfDeltaCalculationRange增量的增量
        /// </summary>
        /// <param name="deltaCalculationRange"></param>
        public void CalcDeltaOfDelta(int deltaOfDeltaCalculationRange = 7)
        {
            if(!deltaCalculated)
            {
                Debug.LogError("请先计算增量");
                return;
            }

            List<CharacterPostCountDay> characterPostCountDays = days
                .OrderBy(d => d.Key)
                .Select(d => d.Value)
                .ToList();

            for (int i = 0; i < characterPostCountDays.Count; i++)
            {
                CharacterPostCountDay currentDay = characterPostCountDays[i];
                foreach (var characterTotalPair in currentDay.characterTotalPairs)
                {
                    characterTotalPair.Value.deltaOfDeltaCalculationRange = deltaOfDeltaCalculationRange;
                    characterTotalPair.Value.deltaOfDeltaCalculated = true;
                    int prevDayIndex = Math.Max(i - deltaOfDeltaCalculationRange, 0);
                    for (int j = prevDayIndex; j <= i; j++) // 寻找第一个有此角色统计结果的天
                    {
                        CharacterPostCountDay prevDay = characterPostCountDays[j];
                        if (prevDay.characterTotalPairs.ContainsKey(characterTotalPair.Key))
                        {
                            characterTotalPair.Value.deltaOfDelta = characterTotalPair.Value.delta - prevDay.characterTotalPairs[characterTotalPair.Key].delta;
                            break;
                        }
                    }
                }
            }

            deltaOfDeltaCalculated = true;
            this.deltaOfDeltaCalculationRange = deltaOfDeltaCalculationRange;
        }
    }
}