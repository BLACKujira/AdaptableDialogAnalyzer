// Sekai.ScenarioSnippetSound
using System;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    [Serializable]
    public class ScenarioSnippetSound
    {
        public enum SoundPlayMode
        {
            CrossFade,
            Stack,
            SpecialSePlay,
            Stop,
            BgmVolume
        }
        public SoundPlayMode PlayMode;
        public string Bgm;
        public string Se;
        public float Volume;
        public string SeBundleName;
        public float Duration;
    }
}