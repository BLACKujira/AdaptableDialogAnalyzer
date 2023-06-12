// Sekai.ScenarioSnippetTalkCharacter
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class ScenarioSnippetTalkCharacter
    {
        public int Character2dId;
        public ScenarioSnippetTalkCharacter(int character2dId)
        {
            Character2dId = character2dId;
        }
    }
}