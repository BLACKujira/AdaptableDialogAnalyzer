namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public static class BanGDreamHelper
    {
        public static int GetCharacterId_Scenario(ScenarioSnippetTalk scenarioSnippetTalk)
        {
            if (scenarioSnippetTalk.talkCharacters == null || scenarioSnippetTalk.talkCharacters.Length <= 0) return 0;
            int characterId = scenarioSnippetTalk.talkCharacters[0].characterId;
            if(characterId < 0 || characterId>35) return 0;
            return characterId;
        }
    }
}