// Sekai.Core.VirtualLive.CharacterRotateEvent
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class CharacterRotateEvent : BaseEvent
    {
        public string RotationKey;
        public float RotationY;
    }
}