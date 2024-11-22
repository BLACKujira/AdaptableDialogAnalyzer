using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public abstract class ObjectMentionCountMutiDialogueEditor : CountDialogueEditor
    {
        public Toggle togHideUnmatched;
        [Header("Settings")]
        public Color colorUnmatched = new Color32(240, 240, 240, 255);
        public Color colorMatched = new Color32(60, 203, 176, 255);
        public Color colorUnidentified = new Color32(246, 107, 176, 255);
        [Header("Prefabs")]
        public SpeechBubbleButton speechBubblePrefab;
        public Window inputIntPrefab;

        protected override SpeechBubbleButton SpeechBubblePrefab => speechBubblePrefab;

        ObjectMentionedCountMutiMatrix mentionedCountMatrix;
        protected ObjectMentionedCountMutiMatrix MentionedCountMatrix => mentionedCountMatrix;

        public virtual void Initialize(ObjectMentionedCountMutiMatrix mentionedCountMutiMatrix)
        {
            this.mentionedCountMatrix = mentionedCountMutiMatrix;

            togHideUnmatched.isOn = hideUnmatched;
            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                hideUnmatched = value;
                Refresh();
            });

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;

            base.Initialize(mentionedCountMutiMatrix);
        }

        protected override void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton)
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            speechBubbleButton.defaultBGColor = colorUnmatched;
            speechBubbleButton.SetData(basicTalkSnippet, false, false);

            ObjectMentionedCountMutiMatrix countMatrix = (ObjectMentionedCountMutiMatrix)CountMatrix;

            if (countMatrix.unidentifiedMentionsRow.HasSerif(basicTalkSnippet.RefIdx))
            {
                speechBubbleButton.iceContent.SetIndividualColor(colorUnidentified);
            }
            if (countMatrix.HasMatched(basicTalkSnippet.RefIdx))
            {
                speechBubbleButton.iceContent.SetIndividualColor(colorMatched);
            }

            speechBubbleButton.button.onClick.AddListener(() =>
            {
                OMCMInputInt oMCMInputInt = window.OpenWindow<OMCMInputInt>(inputIntPrefab);
                oMCMInputInt.Initialize(basicTalkSnippet.content, countMatrix[basicTalkSnippet.speakerId][basicTalkSnippet.refIdx], countMatrix.unidentifiedMentionsRow[basicTalkSnippet.refIdx],
                    (i) =>
                    {
                        countMatrix.unidentifiedMentionsRow.RemoveGrid(basicTalkSnippet.refIdx); // 移除未识别的匹配
                        countMatrix[basicTalkSnippet.speakerId][basicTalkSnippet.refIdx].overrideCount = i; //不使用匹配计数，覆盖计数
                        CountMatrix.HasChanged = true; //标记此矩阵已被修改
                    });
            });

            SpeechBubbleWithIntAndButton speechBubbleWithIntAndButton = speechBubbleButton as SpeechBubbleWithIntAndButton;
            if (speechBubbleWithIntAndButton != null)
            {
                speechBubbleWithIntAndButton.txtInt.text = countMatrix[basicTalkSnippet.speakerId][basicTalkSnippet.refIdx]?.Count.ToString() ?? "0";
            }
        }
    }
}