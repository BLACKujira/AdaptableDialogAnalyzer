using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    public class SingleChapterLoader_TextAsset_ReStage_AdvScenario : SingleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_ReStage_AdvScenario.LoadText(textAsset.text);
    }
}