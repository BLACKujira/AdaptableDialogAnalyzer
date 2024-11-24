using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.DataStructures
{
    [Serializable]
    public class SimpleMentionCountResult
    {
        public List<SimpleMentionCountResultItem> items = new List<SimpleMentionCountResultItem>();

        public SimpleMentionCountResultItem this[int characterID]
        {
            get
            {
                foreach (var item in items)
                {
                    if (item.characterID == characterID)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public SimpleMentionCountResult(List<SimpleMentionCountResultItem> items)
        {
            this.items = items;
        }

        /// <summary>
        /// 获取带有排名的结果
        /// </summary>
        public List<SimpleMentionCountResultItemWithRank> GetResultWithRank()
        {
            // 1. 创建一个新的列表用于存储带有排名的结果
            var rankedResults = new List<SimpleMentionCountResultItemWithRank>();

            // 2. 按照 count 排序，生成排名
            var countRanked = items
                .OrderByDescending(item => item.count)
                .Select((item, index) => new { Item = item, Rank = index + 1 })
                .ToList();

            // 3. 按照 Percent 排序，生成百分比排名
            var percentRanked = items
                .OrderByDescending(item => item.Percent)
                .Select((item, index) => new { Item = item, Rank = index + 1 })
                .ToDictionary(x => x.Item.characterID, x => x.Rank);

            // 4. 合并排名信息并生成最终结果
            foreach (var rankedItem in countRanked)
            {
                var itemWithRank = new SimpleMentionCountResultItemWithRank(rankedItem.Item)
                {
                    rank = rankedItem.Rank, // count 的排名
                    percentRank = percentRanked[rankedItem.Item.characterID] // 百分比的排名
                };
                rankedResults.Add(itemWithRank);
            }

            return rankedResults;
        }
    }

    [Serializable]
    public class SimpleMentionCountResultItem
    {
        public int characterID;
        public int count;
        public int serifCount;

        public SimpleMentionCountResultItem(int characterID, int count, int serifCount)
        {
            this.characterID = characterID;
            this.count = count;
            this.serifCount = serifCount;
        }

        protected SimpleMentionCountResultItem() { }

        public float Percent => (float)count / serifCount;
    }

    public class SimpleMentionCountResultItemWithRank : SimpleMentionCountResultItem
    {
        public int rank;
        public int percentRank;

        public SimpleMentionCountResultItemWithRank(SimpleMentionCountResultItem simpleMentionCountResultItem)
        {
            this.characterID = simpleMentionCountResultItem.characterID;
            this.count = simpleMentionCountResultItem.count;
            this.serifCount = simpleMentionCountResultItem.serifCount;
        }
    }
}
