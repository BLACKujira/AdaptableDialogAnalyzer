// Sekai.Core.VirtualLive.CharacterTalkEvent
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class CharacterTalkEvent : BaseEvent
    {
        public string Serif;
        public string VoiceKey;
        public CharacterTalkEvent()
        {
        }
    }
}