using AdaptableDialogAnalyzer.Unity;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public static class BanGDreamHelper
    {
        public static int GetCharacterId_Scenario(ScenarioSnippetTalk scenarioSnippetTalk)
        {
            if (scenarioSnippetTalk.talkCharacters == null || scenarioSnippetTalk.talkCharacters.Length <= 0) return 0;
            int characterId = scenarioSnippetTalk.talkCharacters[0].characterId;
            if (!GlobalConfig.CharacterDefinition.HasDefinition(characterId)) return 0;
            return characterId;
        }

        public static EventStoryInfo GetEventStoryInfo(string chapterId)
        {
            string scenarioevent = "Scenarioevent";
            if (!chapterId.StartsWith(scenarioevent)) return null;

            string ids = chapterId.Substring(scenarioevent.Length);
            string[] idArray = ids.Split('-');

            if (idArray.Length != 2) return null;
            if(!int.TryParse(idArray[0],out int eventId)) return null;
            if(!int.TryParse(idArray[1],out int chapter)) return null;

            return new EventStoryInfo(eventId, chapter);
        }
    }

    public class EventStoryInfo
    {
        public int eventId;
        public int chapter;

        public EventStoryInfo(int eventId, int chapter)
        {
            this.eventId = eventId;
            this.chapter = chapter;
        }
    }
}