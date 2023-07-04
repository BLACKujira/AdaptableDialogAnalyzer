// ScenarioSnippetTalkVoice
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetTalkVoice
    {
        public int characterId;
        public string voiceId;
        public float volume;

        public ScenarioSnippetTalkVoice(int characterId, string voiceId, float volume)
        {
        }
    }
}