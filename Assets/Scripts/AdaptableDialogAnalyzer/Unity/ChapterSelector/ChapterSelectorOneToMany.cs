using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 多角色模式选择器
    /// </summary>
    public class ChapterSelectorOneToMany : ChapterSelector
    {
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;

        int speakerId;

        public void Initialize(MentionedCountManager mentionedCountManager, int speakerId)
        {
            this.speakerId = speakerId;

            Initialize(mentionedCountManager);
        }

        protected override List<CountMatrix> FilterCountMatrices(List<CountMatrix> countMatrices)
        {
            countMatrices = countMatrices
                .Where(cm => cm is MentionedCountMatrix mcm && mcm[speakerId] != null && mcm[speakerId].serifCount > 0)
                .ToList();
            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 多角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name}";
        }

        protected override void InitializeChapterItem(CountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            MentionedCountMatrix mentionedCountMatrix = (MentionedCountMatrix)countMatrix;

            Vector2Int[] mentionedCountArray = mentionedCountMatrix[speakerId].MentionedCountArray;
            mentionedCountArray = mentionedCountArray
                .Where(v2 => v2.y > 0)
                .OrderBy(v2 => -v2.y)
                .ToArray();

            chapterItem.SetData(countMatrix.chapterInfo, mentionedCountMatrix[speakerId].serifCount, mentionedCountArray);

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorOneToMany dialogueEditor = window.OpenWindow<MentionCountDialogueEditorOneToMany>(dialogueEditorPrefab);
                dialogueEditor.Initialize(mentionedCountMatrix, speakerId);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }
    }
}