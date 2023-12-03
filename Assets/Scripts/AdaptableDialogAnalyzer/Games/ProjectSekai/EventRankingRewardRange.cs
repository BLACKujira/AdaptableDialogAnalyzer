// Sekai.EventRankingRewardRange

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [System.Serializable]
    public class EventRankingRewardRange
    {
        public int id;
        public int eventId;
        public int fromRank;
        public int toRank;
        public EventRankingReward[] eventRankingRewards;
    }
}