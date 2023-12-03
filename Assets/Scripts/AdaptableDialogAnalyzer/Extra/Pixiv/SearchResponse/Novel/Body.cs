using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Novel
{
    [Serializable]
    public class Body
    {
        public Novel novel;
        public List<string> relatedTags;
        public TagTranslation tagTranslation;
        public ZoneConfig zoneConfig;
        public ExtraData extraData;
    }
}
