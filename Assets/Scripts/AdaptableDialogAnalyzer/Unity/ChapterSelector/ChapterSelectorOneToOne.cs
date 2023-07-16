using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 单角色模式选择器
    /// </summary>
    public class ChapterSelectorOneToOne : ChapterSelector
    {
        public Toggle togHideUnmatched;
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;

        int speakerId;
        int mentionedPersonId;

        public void Initialize(MentionedCountManager mentionedCountManager, int speakerId, int mentionedPersonId)
        {
            this.speakerId = speakerId;
            this.mentionedPersonId = mentionedPersonId;

            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });

            Initialize(mentionedCountManager);
        }

        protected override List<MentionedCountMatrix> FilterCountMatrices(List<MentionedCountMatrix> countMatrices)
        {
            countMatrices = countMatrices.Where(cm => cm[speakerId] != null && cm[speakerId].serifCount > 0).ToList();

            if (togHideUnmatched.isOn)
            {
                countMatrices = countMatrices.Where(cm => cm[speakerId, mentionedPersonId] != null && cm[speakerId, mentionedPersonId].Count > 0).ToList();
            }

            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 单角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name} | {GlobalConfig.CharacterDefinition[mentionedPersonId].name}";
        }

        protected override void InitializeChapterItem(MentionedCountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            Vector2Int vector2Int = new Vector2Int(mentionedPersonId, countMatrix[speakerId, mentionedPersonId]?.Count ?? 0);

            if (vector2Int.y > 0) chapterItem.SetData(countMatrix.Chapter, countMatrix[speakerId].serifCount, vector2Int);
            else chapterItem.SetData(countMatrix.Chapter, countMatrix[speakerId].serifCount);

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorOneToOne dialogueEditor = window.OpenWindow<MentionCountDialogueEditorOneToOne>(dialogueEditorPrefab);
                dialogueEditor.Initialize(countMatrix, speakerId, mentionedPersonId);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }
    }
}