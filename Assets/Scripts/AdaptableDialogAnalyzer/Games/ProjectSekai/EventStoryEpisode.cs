// Sekai.EventStoryEpisode
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class EventStoryEpisode
    {
        public int id;
        public int eventStoryId;
        public int episodeNo;
        public string title;
        public string assetbundleName;
        public string scenarioId;
        public int releaseConditionId;
        public EpisodeReward[] episodeRewards;
    }
}