// Sekai.Core.VirtualLive.BGMEvent
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class BGMEvent : BaseEvent
    {
        public enum BGMPlayState
        {
            ON,
            OFF
        }
        public string BGMKey;
        public BGMPlayState Play;
    }
}