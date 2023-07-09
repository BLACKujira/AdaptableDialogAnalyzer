// BackstageTalkSnippet
using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class BackstageTalkSnippet
    {
        public uint talkId;
        public List<uint> characterIds;
        public string voiceId;
        public string talkText;
        public string lipAnimation;
        public BackstageTalkFontColorType fontColorType;
        public BackstageTalkFontSizeType fontSizeType;
        public BackstageTalkBalloonType balloonType;
    }
}