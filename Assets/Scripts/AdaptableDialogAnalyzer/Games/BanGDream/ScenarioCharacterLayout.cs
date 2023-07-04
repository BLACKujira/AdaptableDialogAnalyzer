// ScenarioCharacterLayout
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioCharacterLayout
    {
        public enum Side
        {
            None,
            Left,
            LeftOver,
            LeftInside,
            Center,
            Right,
            RightOver,
            RightInside,
            LeftUnder,
            LeftInsideUnder,
            CenterUnder,
            RightUnder,
            RightInsideUnder
        }

        public Side side;
        public int characterId;
        public string costumeType;
        public string motionName;
        public string expressionName;
        public float offsetX;

        public ScenarioCharacterLayout(Side side, int characterId)
        {
        }

        public ScenarioCharacterLayout(Side side, int characterId, string costumeType, string motionName, string expressionName, float offsetX)
        {
        }
    }
}