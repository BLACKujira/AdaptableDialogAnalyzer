using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse
{
    [Serializable]
    public class MergedResponse : MonoBehaviour
    {
        public List<Artwork.DataItem> artworks = new List<Artwork.DataItem>();
        public List<Novel.DataItem> novels = new List<Novel.DataItem>();
    }
}
