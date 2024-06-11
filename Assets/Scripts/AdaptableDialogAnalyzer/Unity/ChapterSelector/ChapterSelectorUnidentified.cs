using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 多义昵称模式选择器
    /// </summary>
    public class ChapterSelectorUnidentified : ChapterSelector
    {
        public Toggle togHideUnmatched;
        [Header("Prefabs")]
        public Window dialogueEditorPrefab;

        string unidentifiedNickname;

        public void Initialize(MentionedCountManager mentionedCountManager, string unidentifiedNickname)
        {
            this.unidentifiedNickname = unidentifiedNickname;

            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                Refresh();
            });

            Initialize(mentionedCountManager);
        }

        protected override List<CountMatrix> FilterCountMatrices(List<CountMatrix> countMatrices)
        {
            if (togHideUnmatched.isOn)
            {
                countMatrices = countMatrices.Where(cm => cm is MentionedCountMatrix mcm && mcm.GetUnidentifiedMentions(unidentifiedNickname) != null).ToList();
            }
            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 多义昵称模式 | {unidentifiedNickname}";
        }

        protected override void InitializeChapterItem(CountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            MentionedCountMatrix mentionedCountMatrix = countMatrix as MentionedCountMatrix;

            UnidentifiedMentions unidentifiedMentions = mentionedCountMatrix.GetUnidentifiedMentions(unidentifiedNickname);

            if (unidentifiedMentions != null) chapterItem.SetData(countMatrix.chapterInfo, mentionedCountMatrix.GetSerifCount(), new Vector2Int(0, unidentifiedMentions.Count));
            else chapterItem.SetData(countMatrix.Chapter, mentionedCountMatrix.GetSerifCount());

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorUnidentified dialogueEditor = window.OpenWindow<MentionCountDialogueEditorUnidentified>(dialogueEditorPrefab);
                dialogueEditor.Initialize(mentionedCountMatrix, unidentifiedNickname);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }
    }
}