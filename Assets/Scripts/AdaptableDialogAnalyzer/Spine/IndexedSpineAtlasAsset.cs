#if USE_SPINE
using Spine.Unity;

namespace AdaptableDialogAnalyzer.Spine
{
    [System.Serializable]
    public class IndexedSpineAtlasAsset
    {
        public int id;
        public SpineAtlasAsset spineAtlasAsset;
    }
}
#endif