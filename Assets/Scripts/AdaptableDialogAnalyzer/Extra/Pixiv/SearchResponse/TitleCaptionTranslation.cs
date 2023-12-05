using ProtoBuf;
using System;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse
{
    [Serializable]
    [ProtoContract]
    public class TitleCaptionTranslation
    {
        [ProtoMember(1)]
        public string workTitle;
        [ProtoMember(2)]
        public string workCaption;
    }
}