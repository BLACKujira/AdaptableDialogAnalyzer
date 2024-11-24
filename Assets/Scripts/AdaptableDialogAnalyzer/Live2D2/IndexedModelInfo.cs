using System;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [Serializable]
    public class IndexedModelInfo
    {
        public int index;
        public ModelInfo modelInfo;

        public IndexedModelInfo()
        {
        }

        public IndexedModelInfo(int index, ModelInfo modelInfo)
        {
            this.index = index;
            this.modelInfo = modelInfo;
        }
    }
}