using System;
using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [Serializable]
    public class Live2D2AnimationSequence
    {
        public List<Live2D2Animation> animationSequence = new List<Live2D2Animation>();

        public Live2D2Animation this[int index] => animationSequence[index];
    }
}