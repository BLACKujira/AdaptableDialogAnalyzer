// ActionSetPath
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ActionSetPath
    {
        public enum ActionType
        {
            Enter,
            Move,
            Stay,
            Exit,
            HideStay
        }

        public ActionType type;
        public IntVector2 cellPosition;
        public int waitActionSetId;
        public int waitCharacterId;
        public int waitPathIndex;
        public float oneCellMoveTick;
        public float durationTick;
        public CharacterDirection direction;
        public string motionName;
        public float tickToPlayProgress;
        public short actionDataIndex;
        public short balloonIndex;
        public short emoteIndex;
        public short areaItemIndex;
    }
}