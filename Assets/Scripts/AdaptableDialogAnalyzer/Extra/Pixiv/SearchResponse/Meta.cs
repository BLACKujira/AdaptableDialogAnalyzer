using System;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse
{
    [Serializable]
    public class Meta
    {
        public string title;
        public string description;
        public string canonical;
        public AlternateLanguages alternateLanguages;
        public string descriptionHeader;
    }
}
