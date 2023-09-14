// ActionSetEmote
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ActionSetEmote
    {
        public enum EmoteType
        {
            None,
            Talk,
            Joy,
            Shy,
            DarkAura,
            Heart,
            Balloon,
            Star,
            Onpu,
            Loud,
            Guiter,
            Drum,
            Question,
            Sweat,
            Depressed
        }

        public enum PlayType
        {
            Tick,
            Infinity
        }

        public EmoteType emoteType;
        public PlayType playType;
        public float delayTick;
        public float showTick;

        public bool IsSame(EmoteType emoteType, float delayTick, float showTick)
        {
            return default;
        }
        public bool IsSame(ActionSetEmote emote)
        {
            return default;
        }
    }
}