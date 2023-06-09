// Sekai.ScenarioSnippetCharacterLayout
using System;

namespace UniversalADVCounter.Games.ProjectSekai
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
        public enum LayoutMoveSpeedType
        {
            Normal,
            Fast,
            Slow
        }
        public enum LayoutDepthType
        {
            NotSet,
            Front,
            Back
        }
        public ActionType Type;
        public ScenarioCharacterLayout.Side SideFrom;
        public float SideFromOffsetX;
        public ScenarioCharacterLayout.Side SideTo;
        public float SideToOffsetX;
        public LayoutDepthType DepthType;
        public int Character2dId;
        public string CostumeType;
        public string MotionName;
        public string FacialName;
        public LayoutMoveSpeedType MoveSpeedType;
    }
}