using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/Live2D2/IndexedLive2D2AnimationSequenceList")]
    public class IndexedLive2D2AnimationSequenceList : ScriptableObject
    {
        public List<IndexedLive2D2AnimationSequence> indexedLive2D2AnimationSequences = new List<IndexedLive2D2AnimationSequence>();

        public IndexedLive2D2AnimationSequence this[int index] => indexedLive2D2AnimationSequences.Where(i=>i.index ==  index).FirstOrDefault();
    }
}