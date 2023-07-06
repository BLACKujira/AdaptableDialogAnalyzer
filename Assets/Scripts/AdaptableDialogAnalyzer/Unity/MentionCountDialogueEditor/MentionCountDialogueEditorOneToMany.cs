﻿using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorOneToMany : MentionCountDialogueEditorToMany
    {
        int speakerId;

        public void Initialize(MentionedCountMatrix mentionedCountMatrix, int speakerId)
        {
            this.speakerId = speakerId;

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
            talkSnippets = talkSnippets.Where(ts => ts.SpeakerId == speakerId).ToList();

            if (togHideUnmatched.isOn)
            {
                talkSnippets = talkSnippets.Where(ts => SnippetCountDictionary[ts.RefIdx].Count > 0 || MentionedCountMatrix.HasUnidentifiedMention(ts.RefIdx)).ToList();
            }

            return talkSnippets;
        }

        protected override string GetTip()
        {
            return $"标记对话 | 多角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name}";
        }
    }
}