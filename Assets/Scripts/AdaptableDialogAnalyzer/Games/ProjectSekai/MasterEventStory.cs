// Sekai.MasterEventStory
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class MasterEventStory
    {
        public int id;
        public int eventId;
        public int bannerGameCharacterUnitId;
        public string assetbundleName;
        public EventStoryEpisode[] eventStoryEpisodes;
    }
}