using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorOneToMany : MentionCountDialogueEditorToMany
    {
        int speakerId;

        public void Initialize(MentionedCountMatrix mentionedCountMatrix,int speakerId)
        {
            this.speakerId = speakerId;

            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
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
            List<Character> characters = GlobalConfig.CharacterDefinition.characters;
            return $"标记对话 | 多角色模式 | {characters[speakerId].name}";
        }
    }
}