#if USE_SPINE
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Spine
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/Spine/IndexedSpineAtlasList")]
    public class IndexedSpineAtlasList : ScriptableObject
    {
        public List<IndexedSpineAtlasAsset> spineAtlasAssets;
        public SpineAtlasAsset defaultSpineAtlasAsset;

        public SpineAtlasAsset this[int index]
        {
            get
            {
                SpineAtlasAsset spineAtlasAsset = TryGetSpineAtlasAsset(index);
                if(spineAtlasAsset == null) 
                {
                    spineAtlasAsset = defaultSpineAtlasAsset;
                    Debug.LogWarning($"不存在id为{index}的spineAtlasAsset");
                }
                return spineAtlasAsset;
            }
        }

        /// <summary>
        /// 当不存在时返回null
        /// </summary>
        public SpineAtlasAsset? TryGetSpineAtlasAsset(int atlasId)
        {
            foreach (var indexedSpineAtlasAsset in spineAtlasAssets)
            {
                if (indexedSpineAtlasAsset.id == atlasId) 
                    return indexedSpineAtlasAsset.spineAtlasAsset;
            }
            return null;
        }
    }
}
#endif