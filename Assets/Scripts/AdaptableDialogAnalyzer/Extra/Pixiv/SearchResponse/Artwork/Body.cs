using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Artwork
{
    [Serializable]
    public abstract class Body
    {
        public Popular popular;
        public List<string> relatedTags;
        public TagTranslation tagTranslation;
        public ZoneConfig zoneConfig;
        public ExtraData extraData;
    }

    [Serializable]
    public class BodyIllustManga : Body
    {
        public Artwork illustManga;
    }

    [Serializable]
    public class BodyIllust : Body
    {
        public Artwork illust;
    }

    [Serializable]
    public class BodyManga : Body
    {
        public Artwork manga;
    }
}