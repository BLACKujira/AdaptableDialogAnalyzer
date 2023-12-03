using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Artwork
{
    [Serializable]
    public class DataItem
    {
        public string id;
        public string title;
        public int illustType;
        public int xRestrict;
        public int restrict;
        public int sl;
        public string url;
        public string description;
        public List<string> tags;
        public string userId;
        public string userName;
        public int width;
        public int height;
        public int pageCount;
        public string isBookmarkable;
        public string bookmarkData;
        public string alt;
        public TitleCaptionTranslation titleCaptionTranslation;
        public string createDate;
        public string updateDate;
        public string isUnlisted;
        public string isMasked;
        public int aiType;
        public string profileImageUrl;
    }
}