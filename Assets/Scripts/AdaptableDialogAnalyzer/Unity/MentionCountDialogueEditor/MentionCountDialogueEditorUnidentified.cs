using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorUnidentified : MentionCountDialogueEditorToMany
    {
        string unidentifiedNickname;

        public void Initialize(MentionedCountMatrix mentionedCountMatrix, string unidentifiedNickname)
        {
            this.unidentifiedNickname = unidentifiedNickname;

            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
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
    }
}