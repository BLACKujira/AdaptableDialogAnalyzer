namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class ChapterTitleGetter
    {
        SuiteMasterGetResponse suiteMasterGetResponse;

        public ChapterTitleGetter(SuiteMasterGetResponse suiteMasterGetResponse) 
        {
            this.suiteMasterGetResponse = suiteMasterGetResponse;
        }

        public string GetChapterTitle(string chapterType, string chapterId)
        {
            switch (chapterType)
            {
                case ChapterLoader_Folder_BanGDream_Scenario.TYPE_CARDSTORY: return GetChapterTitle_Card(chapterId);
                default:
                    return chapterId;
            }
        }

        string GetChapterTitle_Card(string chapterId)
        {
            foreach (var masterCharacterSituation in suiteMasterGetResponse.MasterCharacterSituationMap.Entries)
            {
                if(masterCharacterSituation.Value == null || masterCharacterSituation.Value.Episodes == null) continue;
                foreach (var masterEpisode in masterCharacterSituation.Value.Episodes.Entries)
                {
                    if (chapterId.Equals($"Scenario{masterEpisode.ScenarioId}"))
                    {
                        string episodeType = masterEpisode.EpisodeType.Equals("standard") ? "前篇" : "后篇";
                        return $"{masterEpisode.Title} (卡片 {masterCharacterSituation.Value.Prefix} {episodeType}剧情)";
                    }
                }
            }
            return chapterId;
        }
    }
}