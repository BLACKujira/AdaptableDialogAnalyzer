using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public abstract class ObjectMentionCountDialogueEditor : CountDialogueEditor
    {
        public Toggle togHideUnmatched;
        [Header("Settings")]
        public Color colorWhite = new Color32(240, 240, 240, 255);
        public Color colorBlack = new Color32(68, 68, 102, 255);
        [Header("Prefabs")]
        public SpeechBubbleButton speechBubblePrefab;
        public Window editorFullPrefab;

        Color colorUnmatched;

        protected override SpeechBubbleButton SpeechBubblePrefab => speechBubblePrefab;

        ObjectMentionedCountMatrix mentionedCountMatrix;
        protected ObjectMentionedCountMatrix MentionedCountMatrix => mentionedCountMatrix;

        public virtual void Initialize(ObjectMentionedCountMatrix mentionedCountMatrix)
        {
            this.mentionedCountMatrix = mentionedCountMatrix;

            togHideUnmatched.isOn = hideUnmatched;
            togHideUnmatched.onValueChanged.AddListener((value) =>
            {
                hideUnmatched = value;
                Refresh();
            });

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            colorUnmatched = APCA.GetBlackOrWhite(GlobalColor.ThemeColor, colorWhite, colorBlack);

            base.Initialize(mentionedCountMatrix);
        }

        protected override void InitializeSpeechBubble(BasicTalkSnippet basicTalkSnippet, SpeechBubbleButton speechBubbleButton)
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            speechBubbleButton.defaultBGColor = colorUnmatched;
            speechBubbleButton.SetData(basicTalkSnippet, false, false);

            ObjectMentionedCountMatrix countMatrix = (ObjectMentionedCountMatrix)CountMatrix;

            if (countMatrix.HasMatched(basicTalkSnippet.RefIdx))
            {
                speechBubbleButton.iceContent.SetIndividualColor(GlobalColor.ThemeColor);
            }

            speechBubbleButton.button.onClick.AddListener(() =>
            {
                if (countMatrix.HasMatched(basicTalkSnippet.RefIdx))
                {
                    countMatrix[basicTalkSnippet.SpeakerId].RemoveMatchedDialogue(basicTalkSnippet.RefIdx);
                    speechBubbleButton.iceContent.SetIndividualColor(speechBubbleButton.defaultBGColor);
                }
                else
                {
                    countMatrix.AddMatchedDialogue(basicTalkSnippet.SpeakerId, basicTalkSnippet.RefIdx);
                    speechBubbleButton.iceContent.SetIndividualColor(GlobalColor.ThemeColor);
                }

                //标记此矩阵已被修改
                CountMatrix.HasChanged = true;
            });
        }
    }
}