// BackstageTalkSet
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    [Serializable]
    public class BackstageTalkSet
    {
        public uint backstageTalkSetId;
        public List<BackstageTalkSnippet> snippets;
    }
}