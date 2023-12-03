using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Novel
{
    [Serializable]
    public class DataItem
    {
        public string id;
        public string title;
        public int xRestrict;
        public int restrict;
        public string url;
        public List<string> tags;
        public string userId;
        public string userName;
        public string profileImageUrl;
        public int textCount;
        public int wordCount;
        public int readingTime;
        public string useWordCount;
        public string description;
        public string isBookmarkable;
        public string bookmarkData;
        public int bookmarkCount;
        public string isOriginal;
        public string marker;
        public TitleCaptionTranslation titleCaptionTranslation;
        public string createDate;
        public string updateDate;
        public string isMasked;
        public string seriesId;
        public string seriesTitle;
        public string isUnlisted;
        public int aiType;
        public string genre;
    }
}
