// Sekai.Core.VirtualLive.CharacterUnspawnEvent
using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class CharacterUnspawnEvent : BaseEvent, IMCEffect
    {
        [SerializeField]
        private string effectKey;
        public string EffectKey
        {
            get
            {
                return effectKey;
            }
            set
            {
                effectKey = value;
            }
        }
    }
}