// ScenarioSnippetTalk
using System;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class ScenarioSnippetTalk
    {
        public enum Tension
        {
            Normal
        }

        public enum MotionChangeFactor
        {
            Text,
            PlayTime
        }

        public enum LipSyncMode
        {
            Text,
            Voice,
            Close
        }

        public ScenarioSnippetTalkCharacter[] talkCharacters;
        public string windowDisplayName;
        public string body;
        public Tension tention;
        public LipSyncMode lipSyncMode;
        public MotionChangeFactor motionChangeFactor;
        public ScenarioSnippetTalkMotion[] motions;
        public ScenarioSnippetTalkVoice[] voices;
        public float speed;
        public int fontSize;
        public bool whenFinishCloseWindow;
        public bool requirePlayEffect;
        public int effectReferenceIdx;
        public bool requirePlaySound;
        public int soundReferenceIdx;
        public bool whenStartHideWindow;

        public bool HasVoice()
        {
            return default;
        }
    }
}