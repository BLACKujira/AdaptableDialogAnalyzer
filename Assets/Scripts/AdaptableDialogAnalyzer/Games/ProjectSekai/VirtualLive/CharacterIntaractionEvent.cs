// Sekai.Core.VirtualLive.CharacterIntaractionEvent
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class CharacterIntaractionEvent : BaseEvent
    {
        public enum InteractionActionType
        {
            PenlightSwing
        }

        public InteractionActionType InteractionType;
    }
}