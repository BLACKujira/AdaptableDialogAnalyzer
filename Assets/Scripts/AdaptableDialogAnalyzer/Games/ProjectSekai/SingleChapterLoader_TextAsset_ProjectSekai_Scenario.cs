using AdaptableDialogAnalyzer.Games.ProjectSekai;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 作为Scenario读取TextAsset（除了虚拟演唱会和系统语音之外的基本都是Scenario）
    /// </summary>
    public class SingleChapterLoader_TextAsset_ProjectSekai_Scenario : SingleChapterLoader
    {
        public TextAsset textAsset;

        public override Chapter GetChapter() => Chapter_ProjectSekai_Scenario.LoadText(textAsset.text);
    }
}