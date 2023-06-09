// Sekai.ScenarioSnippet
using System;

namespace UniversalADVCounter.Games.ProjectSekai
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
        public ActionType Action;
        public ProgressType ProgressBehavior;
        public int ReferenceIndex;
        public float Delay;
    }
}