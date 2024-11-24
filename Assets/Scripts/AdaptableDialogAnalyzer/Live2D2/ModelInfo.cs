using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [Serializable]
    public class ModelInfo
    {
        public TextAsset mocFile;
        public TextAsset physicsFile;
        public Texture2D[] textureFiles;
        public float scaleVolume = 8;
        public bool smoothing = true;
    }
}