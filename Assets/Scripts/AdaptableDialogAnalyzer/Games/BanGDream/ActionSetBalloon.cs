// ActionSetBalloon
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ActionSetBalloon
    {
        public enum BalloonEmoteType
        {
            None,
            Talk,
            Joy,
            Shy,
            DarkAura,
            Question,
            Sweat,
            Depressed
        }

        public string message;
        public BalloonEmoteType emoteType;
        public string voiceId;
        public float delayTick;
        public float showTick;

        public bool IsSame(string message, string voiceId, float delayTick, float showTick)
        {
            return default;
        }

        public bool IsSame(ActionSetBalloon balloon)
        {
            return default;
        }

        public ActionSetBalloon()
        {
        }
    }
}