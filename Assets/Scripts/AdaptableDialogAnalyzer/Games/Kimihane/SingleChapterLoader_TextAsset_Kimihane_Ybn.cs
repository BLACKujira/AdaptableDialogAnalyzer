using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class SingleChapterLoader_TextAsset_Kimihane_Ybn : SingleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_Kimihane_Ybn.LoadText(textAsset.text);
    }
}