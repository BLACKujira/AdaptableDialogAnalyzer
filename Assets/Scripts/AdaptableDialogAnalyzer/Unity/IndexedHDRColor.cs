using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [System.Serializable]
    public class IndexedHDRColor
    {
        public int id;
        [ColorUsage(true,true)]
        public Color color = Color.white;
    }
}