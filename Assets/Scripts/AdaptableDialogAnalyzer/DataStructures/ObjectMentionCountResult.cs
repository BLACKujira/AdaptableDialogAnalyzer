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
        /// ��ȡ���������Ľ��
        /// </summary>
        public List<SimpleMentionCountResultItemWithRank> GetResultWithRank()
        {
            // 1. ����һ���µ��б����ڴ洢���������Ľ��
            var rankedResults = new List<SimpleMentionCountResultItemWithRank>();

            // 2. ���� count ������������
            var countRanked = items
                .OrderByDescending(item => item.count)
                .Select((item, index) => new { Item = item, Rank = index + 1 })
                .ToList();

            // 3. ���� Percent �������ɰٷֱ�����
            var percentRanked = items
                .OrderByDescending(item => item.Percent)
                .Select((item, index) => new { Item = item, Rank = index + 1 })
                .ToDictionary(x => x.Item.characterID, x => x.Rank);

            // 4. �ϲ�������Ϣ���������ս��
            foreach (var rankedItem in countRanked)
            {
                var itemWithRank = new SimpleMentionCountResultItemWithRank(rankedItem.Item)
                {
                    rank = rankedItem.Rank, // count ������
                    percentRank = percentRanked[rankedItem.Item.characterID] // �ٷֱȵ�����
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
