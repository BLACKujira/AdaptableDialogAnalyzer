using ProtoBuf;
using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Novel
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
        public int xRestrict;
        [ProtoMember(4)]
        public int restrict;
        [ProtoMember(5)]
        public string url;
        [ProtoMember(6)]
        public List<string> tags;
        [ProtoMember(7)]
        public string userId;
        [ProtoMember(8)]
        public string userName;
        [ProtoMember(9)]
        public string profileImageUrl;
        [ProtoMember(10)]
        public int textCount;
        [ProtoMember(11)]
        public int wordCount;
        [ProtoMember(12)]
        public int readingTime;
        [ProtoMember(13)]
        public string useWordCount;
        [ProtoMember(14)]
        public string description;
        [ProtoMember(15)]
        public string isBookmarkable;
        [ProtoMember(16)]
        public string bookmarkData;
        [ProtoMember(17)]
        public int bookmarkCount;
        [ProtoMember(18)]
        public string isOriginal;
        [ProtoMember(19)]
        public string marker;
        [ProtoMember(20)]
        public TitleCaptionTranslation titleCaptionTranslation;
        [ProtoMember(21)]
        public string createDate;
        [ProtoMember(22)]
        public string updateDate;
        [ProtoMember(23)]
        public string isMasked;
        [ProtoMember(24)]
        public string seriesId;
        [ProtoMember(25)]
        public string seriesTitle;
        [ProtoMember(26)]
        public string isUnlisted;
        [ProtoMember(27)]
        public int aiType;
        [ProtoMember(28)]
        public string genre;
    }
}
