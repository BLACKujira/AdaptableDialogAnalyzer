// ScenarioSnippetTalkMotion
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetTalkMotion
    {
        public int characterId;
        public string motionName;
        public string expressionName;
        public float timingSyncValue;

        public ScenarioSnippetTalkMotion(int characterId, string motionName, string expressionName, float timingSyncValue)
        {
        }
    }
}