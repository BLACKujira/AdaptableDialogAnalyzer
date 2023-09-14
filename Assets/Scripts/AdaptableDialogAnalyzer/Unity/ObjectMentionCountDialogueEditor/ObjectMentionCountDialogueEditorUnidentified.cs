using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ObjectMentionCountDialogueEditorUnidentified : ObjectMentionCountDialogueEditor
    {
        protected override List<BasicTalkSnippet> FilterTalkSnippets(List<BasicTalkSnippet> talkSnippets)
        {
            HashSet<int> matchedRefIdxSet = new HashSet<int>(MentionedCountMatrix.unidentifiedMentionsRow.matchedIndexes);

            if (togHideUnmatched.isOn)
            {
                return talkSnippets.Where(s => matchedRefIdxSet.Contains(s.refIdx)).ToList();
            }
            else
            {
                return talkSnippets;
            }
        }

        protected override string GetTip()
        {
            return $"标记对话 | 消除歧义";
        }
    }
}