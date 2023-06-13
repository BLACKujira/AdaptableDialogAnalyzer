using AdaptableDialogAnalyzerzer.Games.ReStage;
using System;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    [Serializable]
    public class AdvPageData
    {
        public string background;
        public string cg;
        public int useCG;
        public AdvCharacter character1;
        public AdvCharacter character2;
        public AdvCharacter character3;
        public AdvEmotionType emotion1;
        public AdvEmotionType emotion2;
        public AdvEmotionType emotion3;
        public bool emotionEffect1;
        public bool emotionEffect2;
        public bool emotionEffect3;
        public AdvCharacterBase characterBase1;
        public AdvCharacterBase characterBase2;
        public AdvCharacterBase characterBase3;
        public string name;
        public AdvCharacter nameCharacter;
        public string text;
        public string voiceCueSheet;
        public string voiceCueName;
        public bool voiceStop;
        public string seCueSheet;
        public string seCueName;
        public bool seStop;
        public string bgmCueSheet;
        public string bgmCueName;
        public bool bgmStop;

        private const string PLACE_HOLDER = "[replace]";
        private MasterCharacter _cachedMasterCharacter;
        private MasterCharacter masterCharacter => null;

        public string Name(string replaceText, bool isPreview = false)
        {
            return null;
        }

        public string Text(string replaceText)
        {
            return null;
        }

        public static AdvCharacter GetAdvCharacter(int index)
        {
            return default;
        }
    }
}