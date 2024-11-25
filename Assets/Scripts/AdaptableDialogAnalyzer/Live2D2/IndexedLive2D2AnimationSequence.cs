using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [Serializable]
    public class IndexedLive2D2AnimationSequence
    {
        public int index;
        public List<Live2D2Animation> animationSequence = new List<Live2D2Animation>();

        public IndexedLive2D2AnimationSequence()
        {
        }

        public IndexedLive2D2AnimationSequence(int index, List<Live2D2Animation> animationSequence)
        {
            this.index = index;
            this.animationSequence = animationSequence;
        }
    }
}