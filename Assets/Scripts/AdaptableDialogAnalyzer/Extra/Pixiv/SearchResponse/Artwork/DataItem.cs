using ProtoBuf;
using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Artwork
{
    [Serializable]
    [ProtoContract]
    public class DataItem
    {
        [ProtoMember(1)]
        public string id;
        [ProtoMember(2)]
        public string title;
        [ProtoMember(3)]
        public int illustType;
        [ProtoMember(4)]
        public int xRestrict;
        [ProtoMember(5)]
        public int restrict;
        [ProtoMember(6)]
        public int sl;
        [ProtoMember(7)]
        public string url;
        [ProtoMember(8)]
        public string description;
        [ProtoMember(9)]
        public List<string> tags;
        [ProtoMember(10)]
        public string userId;
        [ProtoMember(11)]
        public string userName;
        [ProtoMember(12)]
        public int width;
        [ProtoMember(13)]
        public int height;
        [ProtoMember(14)]
        public int pageCount;
        [ProtoMember(15)]
        public string isBookmarkable;
        [ProtoMember(16)]
        public string bookmarkData;
        [ProtoMember(17)]
        public string alt;
        [ProtoMember(18)]
        public TitleCaptionTranslation titleCaptionTranslation;
        [ProtoMember(19)]
        public string createDate;
        [ProtoMember(20)]
        public string updateDate;
        [ProtoMember(21)]
        public string isUnlisted;
        [ProtoMember(22)]
        public string isMasked;
        [ProtoMember(23)]
        public int aiType;
        [ProtoMember(24)]
        public string profileImageUrl;
    }
}