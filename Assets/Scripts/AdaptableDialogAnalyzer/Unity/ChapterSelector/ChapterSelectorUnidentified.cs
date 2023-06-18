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

        protected override List<MentionedCountMatrix> FilterCountMatrices(List<MentionedCountMatrix> countMatrices)
        {
            if (togHideUnmatched.isOn)
            {
                countMatrices = countMatrices.Where(cm => cm.GetUnidentifiedMentions(unidentifiedNickname) != null).ToList();
            }
            return countMatrices;
        }

        protected override string GetTip()
        {
            return $"选择剧情 | 多义昵称模式 | {unidentifiedNickname}";
        }

        protected override void InitializeChapterItem(MentionedCountMatrix countMatrix, ChapterSelector_ChapterItem chapterItem)
        {
            UnidentifiedMentions unidentifiedMentions = countMatrix.GetUnidentifiedMentions(unidentifiedNickname);

            if (unidentifiedMentions != null) chapterItem.SetData(countMatrix.Chapter, countMatrix.GetSerifCount(), new Vector2Int(0,unidentifiedMentions.Count));
            else chapterItem.SetData(countMatrix.Chapter, countMatrix.GetSerifCount());

            chapterItem.button.onClick.AddListener(() =>
            {
                MentionCountDialogueEditorUnidentified dialogueEditor = window.OpenWindow<MentionCountDialogueEditorUnidentified>(dialogueEditorPrefab);
                dialogueEditor.Initialize(countMatrix, unidentifiedNickname);
                dialogueEditor.window.OnClose.AddListener(() => Refresh());
            });
        }
    }
}