using System;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse
{
    [Serializable]
    public class MangaRoot : Root
    {
        public Artwork.BodyManga body;
    }
}