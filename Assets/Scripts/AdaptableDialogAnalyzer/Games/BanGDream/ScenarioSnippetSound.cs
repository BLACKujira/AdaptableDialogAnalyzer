// ScenarioSnippetSound
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetSound
    {
        public enum PlayMode
        {
            CrossFade,
            Stack
        }

        public PlayMode playMode;
        public string bgm;
        public string se;
        public float volume;
        public string seBundleName;
        public float duration;

        public ScenarioSnippetSound()
        {
        }
    }
}