using System;

namespace AdaptableDialogAnalyzer.DataStructures
{
    [Serializable]
    public class MatchedSerif
    {
        public int refIdx;
        public int startIndex;
        public int length;

        // 构造函数
        public MatchedSerif(int refIdx, int startIndex, int length)
        {
            this.refIdx = refIdx;
            this.startIndex = startIndex;
            this.length = length;
        }
    }
}