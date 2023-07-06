using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorManyToMany : MentionCountDialogueEditorToMany
    {
        public new void Initialize(MentionedCountMatrix mentionedCountMatrix)
        {
            togHideUnmatched.isOn = hideUnmatched;
            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                hideUnmatched = value;
                Refresh();
            });

            base.Initialize(mentionedCountMatrix);

            Debug.Log(mentionedCountMatrix.Chapter.ExtraInfo);
        }

        protected override List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets)
        {
            if (togHideUnmatched.isOn)
            {
                talkSnippets = talkSnippets.Where(ts => SnippetCountDictionary[ts.RefIdx].Count > 0 || MentionedCountMatrix.HasUnidentifiedMention(ts.RefIdx)).ToList();
            }

            return talkSnippets;
        }

        protected override string GetTip()
        {
            return $"标记对话 | 全角色模式";
        }
    }
}