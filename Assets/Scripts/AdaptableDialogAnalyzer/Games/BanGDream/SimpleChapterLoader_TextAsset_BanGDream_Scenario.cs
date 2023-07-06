using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{

    public class SimpleChapterLoader_TextAsset_BanGDream_Scenario : SimpleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_BanGDream_Scenario.LoadText(textAsset.text);
    }
}