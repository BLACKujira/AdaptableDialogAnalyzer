using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ObjectMentionCountDialogueEditorChara : ObjectMentionCountDialogueEditor
    {
        public Window editorFullPrefab;

        int speakerId;

        public void Initialize(ObjectMentionedCountMatrix mentionedCountMatrix, int speakerId)
        {
            this.speakerId = speakerId;
            Initialize(mentionedCountMatrix);
        }

        protected override List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets)
        {
            HashSet<int> matchedRefIdxSet = MentionedCountMatrix.MatchedRefIdxSet;
            List<BasicTalkSnippet> basicTalkSnippets = talkSnippets.Where(s => s.speakerId == speakerId).ToList();
            
            if(togHideUnmatched.isOn)
            {
                return basicTalkSnippets.Where(s=> matchedRefIdxSet.Contains(s.refIdx)).ToList();
            }
            else
            {
                return basicTalkSnippets;
            }
        }

        protected override string GetTip()
        {
            return $"标记对话 | 单角色模式 | {GlobalConfig.CharacterDefinition[speakerId].name}";
        }
    }
}