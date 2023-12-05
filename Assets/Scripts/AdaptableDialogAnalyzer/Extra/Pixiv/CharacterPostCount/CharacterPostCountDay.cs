using AdaptableDialogAnalyzer.Unity.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class CharacterPostCountDay
    {
        public DateTime date;
        public Dictionary<int, CharacterPostCountDayItem> characterTotalPairs = new Dictionary<int, CharacterPostCountDayItem>();

        public CharacterPostCountDay(DateTime date)
        {
            this.date = date;
        }

        public void Add(int characterId, bool isNsfw, int count = 1)
        {
            if (!characterTotalPairs.ContainsKey(characterId))
            {
                characterTotalPairs[characterId] = new CharacterPostCountDayItem(characterId);
            }
            characterTotalPairs[characterId].total += count;
            if (isNsfw)
            {
                characterTotalPairs[characterId].nsfwCount += count;
            }
        }

        public void AddOrReplace(CharacterPostCountDayItem characterPostCountDayItem)
        {
            characterTotalPairs[characterPostCountDayItem.characterId] = characterPostCountDayItem.Clone();
        }

        public List<IAutoSortBarChartData> GetAutoSortBarChartData()
        {
            return characterTotalPairs
                .Select(kvp => (IAutoSortBarChartData)kvp.Value)
                .ToList();
        }
    }
}