// ScenarioSnippetCharacterLayout
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetCharacterLayout
    {
        public enum ActionType
        {
            None,
            Move,
            Apper,
            Hide,
            ShakeX,
            ShakeY
        }

        public enum MoveSpeedType
        {
            Normal,
            Fast,
            Slow
        }

        public enum DepthType
        {
            NotSet,
            Front,
            Back
        }

        public ActionType type;
        public ScenarioCharacterLayout.Side sideFrom;
        public float sideFromOffsetX;
        public ScenarioCharacterLayout.Side sideTo;
        public float sideToOffsetX;
        public DepthType depthType;
        public int characterId;
        public string costumeType;
        public string motionName;
        public string expressionName;
        public MoveSpeedType moveSpeedType;

        public float MoveSpeed
        {
            get
            {
                return default;
            }
        }

        public float BaseDepth
        {
            get
            {
                return default;
            }
        }

        public ScenarioSnippetCharacterLayout()
        {
        }

        public ScenarioSnippetCharacterLayout(int characterId, ActionType type, ScenarioCharacterLayout.Side sideFrom, ScenarioCharacterLayout.Side sideTo, MoveSpeedType moveSpeedType)
        {
        }
    }
}