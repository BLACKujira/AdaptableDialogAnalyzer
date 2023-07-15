#if USE_SPINE
using AdaptableDialogAnalyzer.Unity;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Spine
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/Spine/SpineAtlasList")]
    public class SpineAtlasList : ScriptableObject
    {
        public List<SpineAtlasAsset> spineAtlasAssets;
        public SpineAtlasAsset this[int index] => spineAtlasAssets[index];
    }
}
#endif