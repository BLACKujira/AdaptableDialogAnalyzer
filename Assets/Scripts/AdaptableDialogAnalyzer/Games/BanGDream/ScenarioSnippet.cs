// ScenarioSnippet
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippet
    {
        public enum ActionType
        {
            None,
            Talk,
            CharacerLayout,
            InputName,
            CharacterMotion,
            Selectable,
            SpecialEffect,
            Sound
        }

        public enum ProgressType
        {
            Now,
            WaitFinished
        }

        public ActionType actionType;
        public ProgressType progressType;
        public int referenceIndex;
        public float delay;
        public bool isWaitForSkipMode;

        public bool ShouldWait(float time, bool isSkipMode)
        {
            return default;
        }
    }
}