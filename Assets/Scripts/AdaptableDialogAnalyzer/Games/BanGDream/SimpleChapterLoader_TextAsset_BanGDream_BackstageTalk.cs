using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class SimpleChapterLoader_TextAsset_BanGDream_BackstageTalk : SimpleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_BanGDream_BackstageTalk.LoadText(textAsset.text);
    }
}