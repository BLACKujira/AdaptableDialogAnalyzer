﻿using AdaptableDialogAnalyzer.Games.Kimihane;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    public class SimpleChapterLoader_TextAsset_ReStage_AdvScenario : SimpleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_ReStage_AdvScenario.LoadText(textAsset.text);
    }
}