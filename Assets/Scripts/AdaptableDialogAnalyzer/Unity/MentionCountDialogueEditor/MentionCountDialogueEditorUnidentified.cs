using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorUnidentified : MentionCountDialogueEditorToMany
    {
        string unidentifiedNickname;
        [Header("Prefabs")]
        public Window editorManyToManyPrefab;

        public void Initialize(MentionedCountMatrix mentionedCountMatrix, string unidentifiedNickname)
        {
            this.unidentifiedNickname = unidentifiedNickname;

            togHideUnmatched.isOn = hideUnmatched;
            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                hideUnmatched = value;
                Refresh();
            });

            Initialize(mentionedCountMatrix);
        }

        protected override List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets)
        {
            if (togHideUnmatched.isOn)
            {
                talkSnippets = talkSnippets.Where(ts => MentionedCountMatrix.HasUnidentifiedMention(ts.RefIdx,unidentifiedNickname)).ToList();
            }

            return talkSnippets;
        }

        protected override string GetTip()
        {
            return $"标记对话 | 多义昵称模式 | {unidentifiedNickname}";
        }

        public void OpenEditorManyToMany()
        {
            MentionCountDialogueEditorManyToMany editorManyToMany = window.OpenWindow<MentionCountDialogueEditorManyToMany>(editorManyToManyPrefab);
            editorManyToMany.Initialize(MentionedCountMatrix);
        }
    }
}