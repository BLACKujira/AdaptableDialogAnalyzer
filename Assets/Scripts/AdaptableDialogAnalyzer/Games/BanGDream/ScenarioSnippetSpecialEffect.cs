// ScenarioSnippetSpecialEffect
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetSpecialEffect
    {
        public enum EffectType
        {
            None,
            BlackIn,
            BlackOut,
            WhiteIn,
            WhiteOut,
            ShakeScreen,
            ShakeWindow,
            ChangeBackground,
            Telop,
            FlashbackIn,
            FlashbackOut,
            ChangeCardStill,
            AmbientColorNormal,
            AmbientColorEvening,
            AmbientColorNight,
            PlayScenarioEffect,
            StopScenarioEffect,
            ChangeBackgroundStill,
            MonochromeIn,
            MonochromeOut,
            SepiaIn,
            SepiaOut
        }

        public EffectType effectType;
        public string stringVal;
        public string stringValSub;
        public float duration;
        public string animationTriggerName;

        public bool ExistAnimationTriggerName
        {
            get
            {
                return default;
            }
        }

        public bool IsDigestTelopEffect
        {
            get
            {
                return default;
            }
        }

        public bool IsSameEffect(ScenarioSnippetSpecialEffect targetEffectData)
        {
            return default;
        }

        public ScenarioSnippetSpecialEffect()
        {
        }
    }
}