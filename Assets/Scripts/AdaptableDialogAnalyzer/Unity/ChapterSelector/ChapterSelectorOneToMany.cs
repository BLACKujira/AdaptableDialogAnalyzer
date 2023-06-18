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

        protected override List<MentionedCountMatrix> FilterCountMatrices(List<MentionedCountMatrix> countMatrices)
        {
            countMatrices = countMatrices.Where(cm => cm[speakerId].SerifCount > 0).ToList();
            return countMatrices;
        }

        protected override string GetTip()
        {
            List<Character> characters = GlobalConfig.CharacterDefinition.characters;
            return $"选择剧情 | 多角色模式 | {characters[speakerId].name}";
        }

        protected override void InitializeChapterItem(MentionedCountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            Vector2Int[] mentionedCountArray = countMatrix[speakerId].MentionedCountArray;
            mentionedCountArray = mentionedCountArray
                .Where(v2=>v2.y>0)
                .OrderBy(v2=>-v2.y)
                .ToArray();

            chapterItem.SetData(countMatrix.Chapter, countMatrix[speakerId].SerifCount,mentionedCountArray);

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorOneToMany dialogueEditor = window.OpenWindow<MentionCountDialogueEditorOneToMany>(dialogueEditorPrefab);
                dialogueEditor.Initialize(countMatrix, speakerId);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }
    }
}