// ActionSetDetail
using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ActionSetDetail
    {
        public enum PlayType
        {
            Once,
            Loop
        }

        public enum ReactionType
        {
            None,
            Scenario,
            Shop
        }

        public uint characterId;
        public string characterResouceId;
        public ReactionType reactionType;
        public string reactionTypeBelongId;
        public PlayType playType;
        public float totalTick;
        public List<ActionSetPath> actionPaths;
        public List<ActionSetBalloon> balloons;
        public List<ActionSetEmote> emotes;
        public List<ActionSetAreaItem> syncAreaItems;
    }
}