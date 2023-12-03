// Sekai.MasterEvent
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class MasterEvent
    {
        public int id;
        public string eventType;
        public string name;
        public string assetbundleName;
        public string bgmAssetbundleName;
        public string eventPointAssetbundleName;
        public long startAt;
        public long aggregateAt;
        public long rankingAnnounceAt;
        public long distributionStartAt;
        public long closedAt;
        public long distributionEndAt;
        public EventRankingRewardRange[] eventRankingRewardRanges;
        public int? virtualLiveId;
        public string unit;
        public GameEventType EventType;
        public DateTime StartTime => ProjectSekaiHelper.UnixTimeMSToDateTimeTST(startAt);
        public DateTime AggregateTime => ProjectSekaiHelper.UnixTimeMSToDateTimeTST(aggregateAt);
        public DateTime EndTime => ProjectSekaiHelper.UnixTimeMSToDateTimeTST(closedAt);
        public UnitType BonusUnitType => throw new NotImplementedException();
    }
}