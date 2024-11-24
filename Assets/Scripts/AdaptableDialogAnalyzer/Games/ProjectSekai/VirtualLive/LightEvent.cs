// Sekai.Core.VirtualLive.LightEvent
using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class LightEvent : BaseEvent
    {
        public enum TargetLight
        {
            Stage,
            Character
        }

        public TargetLight Target;
        public Color LightColor;
        public Color RimColor;
        public float Intensity;
    }
}