using AdaptableDialogAnalyzer.Unity.UIElements;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class CharacterPostCountDayItem: IAutoSortBarChartData
    {
        public int characterId;
        public float total = 0;
        public float nsfwCount = 0;

        public bool deltaCalculated = false;
        public int deltaCalculationRange = -1;
        public float delta = 0;

        public bool deltaOfDeltaCalculated;
        public int deltaOfDeltaCalculationRange = -1;
        public float deltaOfDelta = 0;

        public CharacterPostCountDayItem(int characterId)
        {
            this.characterId = characterId;
        }

        public int Id => characterId;
        public float Value => total;

        public IAutoSortBarChartData Lerp(IAutoSortBarChartData target, float t)
        {
            CharacterPostCountDayItem targetItem = target as CharacterPostCountDayItem;
            CharacterPostCountDayItem result = new CharacterPostCountDayItem(characterId);
            result.total = Mathf.Lerp(total, targetItem.total, t);
            result.nsfwCount = Mathf.Lerp(nsfwCount, targetItem.nsfwCount, t);

            if(deltaCalculated && targetItem.deltaCalculated)
            {
                if(deltaCalculationRange != targetItem.deltaCalculationRange)
                {
                    Debug.LogError("deltaCalculationRange不一致");
                }
                result.delta = Mathf.Lerp(delta, targetItem.delta, t);
            }

            if(deltaOfDeltaCalculated && targetItem.deltaOfDeltaCalculated)
            {
                if(deltaOfDeltaCalculationRange != targetItem.deltaOfDeltaCalculationRange)
                {
                    Debug.LogError("deltaOfDeltaCalculationRange不一致");
                }
                result.deltaOfDelta = Mathf.Lerp(deltaOfDelta, targetItem.deltaOfDelta, t);
            }

            return result;
        }

        public static CharacterPostCountDayItem operator +(CharacterPostCountDayItem a, CharacterPostCountDayItem b)
        {
            CharacterPostCountDayItem result = new CharacterPostCountDayItem(a.characterId);
            result.total = a.total + b.total;
            result.nsfwCount = a.nsfwCount + b.nsfwCount;
            return result;
        }

        public static CharacterPostCountDayItem operator -(CharacterPostCountDayItem a, CharacterPostCountDayItem b)
        {
            CharacterPostCountDayItem result = new CharacterPostCountDayItem(a.characterId);
            result.total = a.total - b.total;
            result.nsfwCount = a.nsfwCount - b.nsfwCount;
            return result;
        }

        public CharacterPostCountDayItem Clone()
        {
            CharacterPostCountDayItem result = new CharacterPostCountDayItem(characterId);
            result.total = total;
            result.nsfwCount = nsfwCount;
            return result;
        }
    }
}