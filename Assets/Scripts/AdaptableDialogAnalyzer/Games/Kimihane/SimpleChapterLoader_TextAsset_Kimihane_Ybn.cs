using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class SimpleChapterLoader_TextAsset_Kimihane_Ybn : SimpleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_Kimihane_Ybn.LoadText(textAsset.text);
    }
}