// ScenarioSnippetSelectable
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetSelectable
    {
        public enum Type
        {
            Talk,
            QuestList
        }

        public Type type;
        public string[] itemTitle;

        public ScenarioSnippetSelectable()
        {
        }
    }
}