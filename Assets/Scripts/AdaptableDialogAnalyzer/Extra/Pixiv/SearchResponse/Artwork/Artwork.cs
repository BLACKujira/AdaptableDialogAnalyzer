using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Artwork
{
    [Serializable]
    public class Artwork
    {
        public List<DataItem> data;
        public int total;
        public int lastPage;
        public List<BookmarkRangesItem> bookmarkRanges;
    }
}