using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse.Artwork
{
    [Serializable]
    public class Popular
    {
        public List<DataItem> recent;
        public List<DataItem> permanent;
    }
}