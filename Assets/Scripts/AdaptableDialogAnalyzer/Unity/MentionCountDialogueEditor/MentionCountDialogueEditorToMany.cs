using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public abstract class MentionCountDialogueEditorToMany : MentionCountDialogueEditor
    {
        public Toggle togHideUnmatched;
        [Header("Settings")]
        public Color colorUnmatched = new Color(240, 240, 240, 255);
        public Color colorMatched = new Color(60, 203, 176, 255);
        public Color colorUnidentified = new Color(246, 107, 176, 255);
        [Header("Prefabs")]
        public SpeechBubbleButton speechBubblePrefab;
        public Window mutiCharacterSelectorPrefab;

        protected override SpeechBubbleButton SpeechBubblePrefab => speechBubblePrefab;

        Dictionary<int, List<int>> snippetCountDictionary = new Dictionary<int, List<int>>();
        protected Dictionary<int, List<int>> SnippetCountDictionary => snippetCountDictionary;

        protected new void Initialize(MentionedCountMatrix mentionedCountMatrix)
        {
            snippetCountDictionary = mentionedCountMatrix.GetSnippetCountDictionary();
            base.Initialize(mentionedCountMatrix);
        }

        protected override void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton)
        {
            speechBubbleButton.defaultBGColor = colorUnmatched;
            speechBubbleButton.SetData(basicTalkSnippet, false, false);

            if (MentionedCountMatrix.HasUnidentifiedMention(basicTalkSnippet.RefIdx)) speechBubbleButton.iceContent.SetIndividualColor(colorUnidentified);
            else if (SnippetCountDictionary[basicTalkSnippet.RefIdx].Count>0) speechBubbleButton.iceContent.SetIndividualColor(colorMatched);
            else speechBubbleButton.iceContent.SetIndividualColor(colorUnmatched);

            SpeechBubbleWithLabels speechBubbleWithLabels = speechBubbleButton as SpeechBubbleWithLabels;
            if (speechBubbleWithLabels == null)
            {
                Debug.LogError("必须使用带角色标签栏的对话气泡");
                return;
            }

            int[] characterIds = snippetCountDictionary[basicTalkSnippet.RefIdx].ToArray();
            speechBubbleWithLabels.SetCharacterLabels(characterIds);
            speechBubbleWithLabels.button.onClick.AddListener(() => 
            {
                int[] oldIds = snippetCountDictionary[basicTalkSnippet.RefIdx].ToArray();
                MutiCharacterSelector mutiCharacterSelector = window.OpenWindow<MutiCharacterSelector>(mutiCharacterSelectorPrefab);
                mutiCharacterSelector.Initialize(oldIds, (ids) =>
                {
                    HashSet<int> oldId = new HashSet<int>(oldIds);
                    HashSet<int> newId = new HashSet<int>(ids);

                    HashSet<int> addId = new HashSet<int>(newId);
                    addId.ExceptWith(oldId);

                    HashSet<int> removeId = new HashSet<int>(oldId);
                    removeId.ExceptWith(newId);

                    foreach (var id in addId)
                    {
                        MentionedCountMatrix[basicTalkSnippet.SpeakerId, id].AddMatchedDialogue(basicTalkSnippet.RefIdx);
                        snippetCountDictionary[basicTalkSnippet.RefIdx].Add(id);
                    }

                    foreach (var id in removeId)
                    {
                        MentionedCountMatrix[basicTalkSnippet.SpeakerId, id].RemoveMatchedDialogue(basicTalkSnippet.RefIdx);
                        snippetCountDictionary[basicTalkSnippet.RefIdx].Remove(id);
                    }

                    MentionedCountMatrix.RemoveUnidentifiedMention(basicTalkSnippet.RefIdx);
                });

                mutiCharacterSelector.window.OnClose.AddListener(() =>
                {
                    MentionedCountMatrix.HasChanged = true;
                    int[] newIds = snippetCountDictionary[basicTalkSnippet.RefIdx].ToArray();
                    speechBubbleWithLabels.SetCharacterLabels(newIds);

                    if (MentionedCountMatrix.HasUnidentifiedMention(basicTalkSnippet.RefIdx)) speechBubbleButton.iceContent.SetIndividualColor(colorUnidentified);
                    else if (SnippetCountDictionary[basicTalkSnippet.RefIdx].Count > 0) speechBubbleButton.iceContent.SetIndividualColor(colorMatched);
                    else speechBubbleButton.iceContent.SetIndividualColor(colorUnmatched);

                    RefreshLayout();
                });
            });
        }
    }
}