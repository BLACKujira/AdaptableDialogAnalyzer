// Sekai.Core.VirtualLive.CharacterMoveEvent
using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class CharacterMoveEvent : BaseEvent
    {
        public Vector3 Position;
        public string MoveKey;
        public float RotationY;
    }
}