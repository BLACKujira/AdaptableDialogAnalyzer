using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Novel
{
    [Serializable]
    public class Novel
    {
        public List<DataItem> data;
        public int total;
        public int lastPage;
        public List<BookmarkRangesItem> bookmarkRanges;
    }
}
