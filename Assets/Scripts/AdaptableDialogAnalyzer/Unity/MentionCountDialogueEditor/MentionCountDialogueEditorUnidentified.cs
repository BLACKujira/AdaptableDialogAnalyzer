using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionCountDialogueEditorUnidentified : MentionCountDialogueEditorToMany
    {
        string unidentifiedNickname;

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
                talkSnippets = talkSnippets.Where(ts => MentionedCountMatrix.HasUnidentifiedMention(ts.RefIdx, unidentifiedNickname)).ToList();
            }

            return talkSnippets;
        }

        protected override string GetTip()
        {
            return $"标记对话 | 多义昵称模式 | {unidentifiedNickname}";
        }

        /// <summary>
        /// 标记此剧情中所有同种类的模糊昵称
        /// </summary>
        public void MarkAllUnidentifiedNicknames()
        {
            MutiCharacterSelector mutiCharacterSelector = window.OpenWindow<MutiCharacterSelector>(mutiCharacterSelectorPrefab);
            mutiCharacterSelector.Initialize(new int[0], (selectedIds) =>
            {
                UnidentifiedMentions unidentifiedMentions = MentionedCountMatrix.unidentifiedMentionsList
                    .Where(l => l.unidentifiedNickname.Equals(unidentifiedNickname))
                    .FirstOrDefault();

                SpeechBubbleWithLabels[] speechBubbleWithLabels = SpeechBubbles
                    .Where(b => unidentifiedMentions.HasSerif(b.BasicTalkSnippet.RefIdx))
                    .Select(b => (SpeechBubbleWithLabels)b)
                    .ToArray();

                int[] refIdxes = unidentifiedMentions.matchedIndexes.ToArray();
                BasicTalkSnippet[] talkSnippets = MentionedCountMatrix.Chapter.TalkSnippets;
                foreach (var refIdx in refIdxes)
                {
                    MentionedCountMatrix.HasChanged = true;

                    int speakerId = talkSnippets
                        .Where(s => s.RefIdx == refIdx)
                        .FirstOrDefault().SpeakerId;

                    foreach (var mentionedPersonId in selectedIds)
                    {
                        MentionedCountMatrix[speakerId, mentionedPersonId].AddMatchedDialogue(refIdx);
                        if (!SnippetCountDictionary[refIdx].Contains(mentionedPersonId)) SnippetCountDictionary[refIdx].Add(mentionedPersonId);
                    }

                    MentionedCountMatrix.RemoveUnidentifiedMention(unidentifiedNickname, refIdx);
                }

                foreach (var speechBubbleWithLabel in speechBubbleWithLabels)
                {
                    BasicTalkSnippet basicTalkSnippet = speechBubbleWithLabel.BasicTalkSnippet;

                    int[] newIds = SnippetCountDictionary[basicTalkSnippet.RefIdx].ToArray();
                    speechBubbleWithLabel.SetCharacterLabels(newIds);

                    if (MentionedCountMatrix.HasUnidentifiedMention(basicTalkSnippet.RefIdx)) speechBubbleWithLabel.iceContent.SetIndividualColor(colorUnidentified);
                    else if (SnippetCountDictionary[basicTalkSnippet.RefIdx].Count > 0) speechBubbleWithLabel.iceContent.SetIndividualColor(colorMatched);
                    else speechBubbleWithLabel.iceContent.SetIndividualColor(colorUnmatched);

                    RefreshLayout();
                }
            });
        }
    }
}