using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 全角色模式选择器
    /// </summary>
    public class ChapterSelectorManyToMany : ChapterSelector
    {
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;

        public new void Initialize(MentionedCountManager mentionedCountManager)
        {
            base.Initialize(mentionedCountManager);
        }

        protected override List<MentionedCountMatrix> FilterCountMatrices(List<MentionedCountMatrix> countMatrices)
        {
            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 全角色模式";
        }

        protected override void InitializeChapterItem(MentionedCountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            chapterItem.SetData(countMatrix.Chapter, countMatrix.GetSerifCount());

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorManyToMany dialogueEditor = window.OpenWindow<MentionCountDialogueEditorManyToMany>(dialogueEditorPrefab);
                dialogueEditor.Initialize(countMatrix);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }
    }
}